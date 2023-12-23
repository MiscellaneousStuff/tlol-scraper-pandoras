﻿#include "ShaderManager.h"

ShaderManager& ShaderManager::GetInstance()
{
    static ShaderManager instance;
    return instance;
}

ShaderManager::~ShaderManager()
{
    Release();
}

Shader* ShaderManager::CreateShader(const std::wstring& shaderPath, const std::wstring& shaderName)
{
    Shader* shader = GetShader(shaderName);
    if(shader == nullptr)
    {
        shader = new Shader(shaderPath, shaderName);
        _shaders[shaderName] = shader;
    }
    return shader;
}


Shader* ShaderManager::CreateShader(const std::wstring& shaderName)
{
    return CreateShader(L"Resources/Shaders", shaderName);
}

Shader* ShaderManager::GetShader(const std::wstring& shaderName)
{
    const auto it = _shaders.find(shaderName);
    if (it != _shaders.end()) {
        return it->second;
    }
    return nullptr;
}

void ShaderManager::Release()
{
    for (auto& pair : _shaders)
    {
        const auto& shader = pair.second;
        shader->Release();
        delete shader;
    }
    _shaders.clear();
}

ShaderManager::ShaderManager()
{
    for (auto& pair : _shaders)
    {
        const auto& shader = pair.second;
        shader->Release();
        delete shader;
    }
    _shaders.clear();
}