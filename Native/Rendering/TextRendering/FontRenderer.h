﻿#pragma once
#include <string>

#include "Font.h"
#include "../Vertex.h"
#include "../Buffers/InstancedBatchBuffer.h"
#include "../Materials/TexturedMaterial.h"
#include <string>

class FontRenderer
{
private:
    TexturedMaterial* _fontMaterial;
    
    Font* _font;
    InstancedBuffer<VertexPositionUv, FontCharacterInstance>* _buffer2D = nullptr;
    InstancedBuffer<VertexPositionUv, FontCharacterInstance>* _buffer3D = nullptr;
    
public:
    FontRenderer(Font* font);

    Vector2 GetTextSize(const std::string& text, float size) const;
    void Draw(const std::string& text, const Vector2& position, const float& scale, const Color& color) const;
    void Draw(const std::string& text, const Vector3& position, const float& scale, const Color& color) const;
    void Flush2D() const;
    void Flush3D() const;

    void Release();
};