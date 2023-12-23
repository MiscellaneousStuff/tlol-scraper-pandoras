﻿#include "Renderer.h"

#include <iostream>
#include "Vertex.h"
#include "../Input/InputManager.h"
#include "../Math/Color.h"
#include "../Menus/Menu.h"
#include "../Menus/SubMenu.h"
#include "glm/ext/matrix_clip_space.hpp"

Renderer* Renderer::_instance = nullptr;
std::once_flag Renderer::_initInstanceFlag;

void RegisterRenderCallback(const RenderCallback callback)
{
    const auto renderer = Renderer::Instance();
    if(renderer == nullptr)
    {
        std::cout << "Renderer doesnt exist" << std::endl;
        return;
    }
    renderer->SetRenderCallback(callback);
}

void RegisterRenderHudCallback(const RenderCallback callback)
{
    Renderer::Instance()->SetRenderHudCallback(callback);
}

void RenderSetClearColor(const Color* color)
{
    Renderer::Instance()->SetClearColor(*color);
}

void RendererRectFilled2D(const Vector2* position, const Vector2* size, const Color* color)
{
    Renderer::Instance()->RectFilled(*position, *size, *color);
}

void RendererRectFilled3D(const Vector3* position, const Vector2* size, const Color* color)
{
    Renderer::Instance()->RectFilled(*position, *size, *color);
}

void RendererRectFilledBordered2D(const Vector2* position, const Vector2* size, const Color* color, const Color* borderColor, const float borderSize)
{
    Renderer::Instance()->RectFilledBordered(*position, *size, *color, *borderColor, borderSize);
}

void RendererRectFilledBordered3D(const Vector3* position, const Vector2* size, const Color* color, const Color* borderColor, const float borderSize)
{
    Renderer::Instance()->RectFilledBordered(*position, *size, *color, *borderColor, borderSize);
}

void RendererRectBorder2D(const Vector2* position, const Vector2* size, const Color* color, const float borderSize)
{
    Renderer::Instance()->RectBorder(*position, *size, *color, borderSize);
}

void RendererRectBorder3D(const Vector3* position, const Vector2* size, const Color* color, const float borderSize)
{
    Renderer::Instance()->RectBorder(*position, *size, *color, borderSize);
}


void RendererCircleFilled2D(const Vector2* position, const float size, const Color* color)
{
    Renderer::Instance()->CircleFilled(*position, size, *color);
}

void RendererCircleFilled3D(const Vector3* position, const float size, const Color* color)
{
    Renderer::Instance()->CircleFilled(*position, size, *color);
}

void RendererCircleFilledBordered2D(const Vector2* position, const float size, const Color* color,
    const Color* borderColor, float borderSize)
{
    Renderer::Instance()->CircleFilledBordered(*position, size, *color, *borderColor, borderSize);
}

void RendererCircleFilledBordered3D(const Vector3* position, const float size, const Color* color,
    const Color* borderColor, float borderSize)
{
    Renderer::Instance()->CircleFilledBordered(*position, size, *color, *borderColor, borderSize);
}

void RendererCircleBorder2D(const Vector2* position, const float size, const Color* color, const float borderSize)
{
    Renderer::Instance()->CircleBorder(*position, size, *color, borderSize);
}

void RendererCircleBorder3D(const Vector3* position, const float size, const Color* color, const float borderSize)
{
    Renderer::Instance()->CircleBorder(*position, size, *color, borderSize);
}


void RendererText2D(const char* text, const Vector2* position, const float size, const Color* color,
                    const TextHorizontalOffset textHorizontalOffset, const TextVerticalOffset textVerticalOffset)
{
    Renderer::Instance()->Text(text, *position, size, *color, textHorizontalOffset, textVerticalOffset);
}

void RendererTextRect2D(const char* text, const Vector2* start, const Vector2* end, const float size, const Color* color,
                    const TextHorizontalOffset textHorizontalOffset, const TextVerticalOffset textVerticalOffset)
{
    Renderer::Instance()->Text(text, *start, *end, size, *color, textHorizontalOffset, textVerticalOffset);
}

void RendererText3D(const char* text, const Vector3* position, float size, const Color* color,
    TextHorizontalOffset textHorizontalOffset, TextVerticalOffset textVerticalOffset)
{
    Renderer::Instance()->Text(text, *position, size, *color, textHorizontalOffset, textVerticalOffset);
}

void RendererSet3DMatrix(const glm::mat4* matrix)
{
    Renderer::Instance()->Set3DMatrix(*matrix);
}

Renderer::Renderer(const HDC hdc) : _hdc(hdc), _viewProjectionMatrix()
{
}

Renderer::~Renderer()
{
    Release();
}

bool Renderer::Init(const int width, const int height)
{
    _width = width;
    _height = height;

    glewInit();
    if (glewInit() != GLEW_OK) {
        std::cerr << "Failed to initialize GLEW" << std::endl;
        return false;
    }

    _rectRenderer = new RectRenderer();
    _circleRenderer = new CircleRenderer();
    _textRenderer = new TextRenderer();

    glEnable(GL_MULTISAMPLE);

    glEnable(GL_BLEND);
    glBlendFuncSeparate(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA, GL_ONE, GL_ONE);
    
    glDisable(GL_DEPTH_TEST);
    glDepthMask(GL_FALSE);
    
    return true;
}

void Renderer::SetRenderCallback(const RenderCallback callback)
{
    _renderCallback = callback;
}

void Renderer::SetRenderHudCallback(RenderCallback callback)
{
    _renderGuiCallback = callback;
}

void Renderer::Text(const std::string& text, const Vector2& position, const float size, const Color& color, const TextHorizontalOffset textHorizontalOffset, const TextVerticalOffset textVerticalOffset) const
{
    _textRenderer->Draw(text, position, size, color, textHorizontalOffset, textVerticalOffset);
}

void Renderer::Text(const std::string& text, const Vector2& start, const Vector2& end, const float size, const Color& color,
                    const TextHorizontalOffset textHorizontalOffset, const TextVerticalOffset textVerticalOffset) const
{
    _textRenderer->Draw(text, start, end, size, color, textHorizontalOffset, textVerticalOffset);
}

void Renderer::Text(const std::string& text, const Vector3& position, float size, const Color& color,
    TextHorizontalOffset textHorizontalOffset, TextVerticalOffset textVerticalOffset) const
{
    _textRenderer->Draw(text, position, size, color, textHorizontalOffset, textVerticalOffset);
}


Renderer* Renderer::CreateInstance(const HDC hdc, const int width, const int height)
{
    Destroy();
        
    _instance = new Renderer(hdc);
    _instance->Init(width, height);
    return _instance;
}

Renderer* Renderer::Instance()
{
    return _instance;
}

void Renderer::Destroy()
{
    if(_instance != nullptr)
    {
        _instance->Release();
        delete _instance;
        _instance=nullptr;
    }
}

glm::mat4 Renderer::Get2DMatrix() const
{
    return glm::ortho(0.0f, static_cast<float>(_width), static_cast<float>(_height), 0.0f, -1.0f, 1.0f);    
}

glm::mat4 Renderer::Get3DMatrix() const
{
    return _viewProjectionMatrix;    
}

void Renderer::Set3DMatrix(const glm::mat4& matrix)
{
    _viewProjectionMatrix = matrix;
}

bool t1;
void Renderer::Render(const float deltaTime)
{
    glClearColor(_clearColor.r, _clearColor.g, _clearColor.b, _clearColor.a);
    glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
    
    if(_renderCallback != nullptr)
    {
        _renderCallback(deltaTime);
    }
    
    _rectRenderer->Flush3D();
    _circleRenderer->Flush3D();
    _textRenderer->Flush3D();
    
    if(_renderGuiCallback != nullptr)
    {
        _renderGuiCallback(deltaTime);
    }
    
    Menu::GetInstance()->Render();


    _textRenderer->Draw("FPS: " + std::to_string(static_cast<int>(1.0f / (deltaTime/1000))), Vector2(10, 26), 26, Color::FromByte(255, 108, 34), TextHorizontalOffset::Left, TextVerticalOffset::Center);
    
    
    _rectRenderer->Flush2D();
    _circleRenderer->Flush2D();
    _textRenderer->Flush2D();
    
    SwapBuffers(_hdc);
}

void Renderer::Release()
{
    if(_rectRenderer != nullptr)
    {
        _rectRenderer->Release();
        delete _rectRenderer;
        _rectRenderer = nullptr;
    }

    if(_circleRenderer != nullptr)
    {
        _circleRenderer->Release();
        delete _circleRenderer;
        _circleRenderer=nullptr;
    }
    
    if(_textRenderer != nullptr)
    {
        _textRenderer->Release();
        delete _textRenderer;
        _textRenderer = nullptr;
    }
}

void Renderer::SetClearColor(const Color color)
{
    _clearColor = color;
}

void Renderer::RectFilled(const Vector2& position, const Vector2& size, const Color& color) const
{
    _rectRenderer->Filled(position, size, color);
}

void Renderer::RectFilled(const Vector3& position, const Vector2& size, const Color& color) const
{
    _rectRenderer->Filled(position, size, color);
}

void Renderer::RectFilledBordered(const Vector2& position, const Vector2& size, const Color& color,
    const Color& borderColor, const float borderSize) const
{
    _rectRenderer->FilledBordered(position, size, color, borderColor, borderSize);
}

void Renderer::RectFilledBordered(const Vector3& position, const Vector2& size, const Color& color,
    const Color& borderColor, const float borderSize) const
{
    _rectRenderer->FilledBordered(position, size, color, borderColor, borderSize);
}

void Renderer::RectBorder(const Vector2& position, const Vector2& size, const Color& color, const float borderSize) const
{
    _rectRenderer->Border(position, size, color, borderSize);
}

void Renderer::RectBorder(const Vector3& position, const Vector2& size, const Color& color, const float borderSize) const
{
    _rectRenderer->Border(position, size, color, borderSize);
}

void Renderer::CircleFilled(const Vector2& position, const float size, const Color& color) const
{
    _circleRenderer->Filled(position, size, color);
}

void Renderer::CircleFilled(const Vector3& position, const float size, const Color& color) const
{
    _circleRenderer->Filled(position, size, color);
}

void Renderer::CircleFilledBordered(const Vector2& position, const float size, const Color& color, const Color& borderColor,
                                    const float borderSize) const
{
    _circleRenderer->FilledBordered(position, size, color, borderColor, borderSize);
}

void Renderer::CircleFilledBordered(const Vector3& position, const float size, const Color& color, const Color& borderColor,
                                    const float borderSize) const
{
    _circleRenderer->FilledBordered(position, size, color, borderColor, borderSize);
}

void Renderer::CircleBorder(const Vector2& position, const float size, const Color& color, const float borderSize) const
{
    _circleRenderer->Border(position, size, color, borderSize);
}

void Renderer::CircleBorder(const Vector3& position, const float size, const Color& color, const float borderSize) const
{
    _circleRenderer->Border(position, size, color, borderSize);
}