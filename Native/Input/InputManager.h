﻿#pragma once
#include <array>
#include <windows.h>
#include <thread>
#include <atomic>
#include <functional>
#include <mutex>
#include <queue>
#include <map>

#include "../Math/Vector2.h"

struct MouseMoveEvent
{
    Vector2 position;
    Vector2 delta;
};

struct KeyStateEvent
{
    unsigned short key;
    bool isDown;
};


extern "C"{
    typedef void (*MouseMoveEventHandler)(MouseMoveEvent mouseMoveEvent);
    typedef void (*KeyStateEventHandler)(KeyStateEvent mouseMoveEvent);
}


class InputManager
{
private:
    static InputManager* _instance;
    static std::once_flag _initInstanceFlag;

    std::thread _hookThread;
    std::atomic<bool> _running;
    std::array<std::atomic<bool>, 256> _keyStates; 
    std::atomic<Vector2> _mousePosition{Vector2(0, 0)};
    std::atomic<Vector2> _mouseMoveDelta{Vector2(0, 0)};

    int _mouseEventListenerId = 1;
    std::map<int, std::function<void(MouseMoveEvent)>> _onMouseMoveEvent;
    
    int keyStateEventId = 1;
    std::map<int, std::function<void(KeyStateEvent)>> _onKeyStateEvent;
    
    HHOOK _keyboardHook;
    HHOOK _mouseHook;

    std::queue<MouseMoveEvent> _mouseMoveEventQueue;
    std::mutex _mouseMoveEventQueueMutex;
    std::condition_variable _mouseMoveEventQueueCondition;

    std::queue<KeyStateEvent> _keyStateEventQueue;
    std::mutex _keyStateEventQueueMutex;
    std::condition_variable _keyStateEventQueueCondition;

    static LRESULT CALLBACK LowLevelKeyboardProc(int nCode, WPARAM wParam, LPARAM lParam);
    static LRESULT CALLBACK LowLevelMouseProc(int nCode, WPARAM wParam, LPARAM lParam);
    
    InputManager();
    
    void HookThreadFunction();
    void UpdateKeyState(unsigned short vkCode, bool isPressed, bool isInjected);
    void PostKeyStateEvent(const KeyStateEvent& event);
    void UpdateMousePosition(float x, float y, bool isInjected);
    void PostMouseMoveEvent(const MouseMoveEvent& event);

    bool _blockUserMouseInput = false;

public:
    InputManager(InputManager const&) = delete;
    void operator=(InputManager const&) = delete;

    static InputManager* GetInstance();
    
    void Start();
    void Stop();
    void Reset();
    bool GetKeyState(unsigned short vkCode) const;
    Vector2 GetMousePosition() const;
    void ProcessInputEvents();

    int AddMouseMoveHandler(MouseMoveEventHandler handler);
    int AddMouseMoveHandler(std::function<void(MouseMoveEvent)> handler);
    void RemoveMouseMoveHandler(int key);
    int AddKeyStateEventHandler(KeyStateEventHandler handler);
    int AddKeyStateEventHandler(std::function<void(KeyStateEvent)> handler);
    void RemoveKeyStateEventHandler(int key);
    void SetBlockUserMouseInput(bool blockInput);

    static INPUT CreateMouseClickInput(unsigned short vkCode, bool down);
    static INPUT CreateMouseMoveInput(const Vector2& position);
    static void MouseSendDown(unsigned short vkCode);
    static void MouseSendUp(unsigned short vkCode);
    static void MouseMove(const Vector2& position);
    static void MouseSend(unsigned short vkCode, bool down);
    static void MouseSend(unsigned short vkCode);

    static INPUT CreateKeyboardInput(unsigned short vkCode, bool down);
    static void KeyboardSendDown(unsigned short vkCode);
    static void KeyboardSendUp(unsigned short vkCode);
    static void KeyboardSend(unsigned short vkCode, bool down);
    static void KeyboardSend(unsigned short vkCode);

    static void SendInputs(INPUT* inputs, unsigned int count);
    
    ~InputManager();
};


extern "C" {
    __declspec(dllexport) void InputManagerReset();
    __declspec(dllexport) bool InputManagerGetKeyState(unsigned short vkCode);
    __declspec(dllexport) void InputManagerGetMousePosition(Vector2* vector);
    
    __declspec(dllexport) int InputManagerAddMouseMoveHandler(MouseMoveEventHandler handler);
    __declspec(dllexport) void InputManagerRemoveMouseMoveHandler(int key);
    __declspec(dllexport) int InputManagerAddKeyStateEventHandler(KeyStateEventHandler handler);
    __declspec(dllexport) void InputManagerRemoveKeyStateEventHandler(int key);

    __declspec(dllexport) void InputManagerSetBlockUserMouseInput(bool blockInput);
    
    __declspec(dllexport) INPUT InputManagerCreateMouseClickInput(unsigned short vkCode, bool down);
    __declspec(dllexport) INPUT InputManagerCreateMouseMoveInput(const Vector2* position);
    __declspec(dllexport) void InputManagerMouseSendDown(unsigned short vkCode);
    __declspec(dllexport) void InputManagerMouseSendUp(unsigned short vkCode);
    __declspec(dllexport) void InputManagerMouseMove(const Vector2* position);
    __declspec(dllexport) void InputManagerMouseSend(unsigned short vkCode);

    __declspec(dllexport) INPUT InputManagerCreateKeyboardInput(unsigned short vkCode, bool down);
    __declspec(dllexport) void InputManagerKeyboardSendDown(unsigned short vkCode);
    __declspec(dllexport) void InputManagerKeyboardSendUp(unsigned short vkCode);
    __declspec(dllexport) void InputManagerKeyboardSend(unsigned short vkCode);
    __declspec(dllexport) void InputManagerSendInputs(INPUT* inputs, unsigned int count);
}
