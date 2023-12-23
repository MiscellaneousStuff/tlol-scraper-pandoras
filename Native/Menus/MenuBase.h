﻿#pragma once
#include <vector>

#include "HotkeyType.h"
#include "../../Input/InputManager.h"
#include "MenuItem.h"

class FloatSlider;
class Toggle;
class SubMenu;
class Hotkey;
class ComboBox;

class MenuBase : public MenuItem
{
protected:
    bool _open = false;
    std::vector<MenuItem*> _items;
    std::vector<Hotkey*> _hotkeys;
    std::vector<MenuBase*> _menus;
    Rect _headerRect;
    Vector2 _nextChildPosition;
    virtual void UpdateNextChildPosition();
    void DrawHeader() const;
    
public:
    MenuBase(MenuItem* parent, MenuItemType menuItemType, const std::string& title, Rect rect);

    MenuBase(const MenuBase&) = delete;
    MenuBase& operator=(const MenuBase&) = delete;
    MenuBase(MenuBase&&) = delete;
    MenuBase& operator=(MenuBase&&) = delete;
    ~MenuBase() override;
    void Render() override;
    void UpdatePosition(const Rect& rect) override;
    void Move(const Vector2& position) override;
    
    SubMenu* AddSubMenu(const std::string& title);
    Toggle* AddToggle(const std::string& title, bool toggled);
    FloatSlider* AddFloatSlider(const std::string& title, float value, float minValue, float maxValue, float step, int precision);
    Hotkey* AddHotkey(const std::string& title, unsigned short hotkey, HotkeyType hotkeyType, bool toggled);
    ComboBox* AddComboBox(const std::string& title, const std::vector<std::string>& items, int selectedIndex);
    
    void AddItem(MenuItem* item);
    void RemoveItem(const MenuItem* itemToRemove);
    bool OnMouseMoveEvent(MouseMoveEvent event) override;
    void ChildOpened(const MenuItem* menuItem) const;
    bool OnKeyStateEvent(KeyStateEvent event) override;
    
    void HandleHotkeys(KeyStateEvent event) const;

    void Close() override;
    virtual Rect GetChildRect(float slots) const;
};


extern "C" {
    __declspec(dllexport) SubMenu* MenuBaseAddSubMenu(MenuBase* instance, const char* title);
    __declspec(dllexport) Toggle* MenuBaseAddToggle(MenuBase* instance, const char* title, bool toggled);
    __declspec(dllexport) FloatSlider* MenuBaseAddFloatSlider(MenuBase* instance, const char* title, float value, float minValue, float maxValue, float step, int precision);
    __declspec(dllexport) Hotkey* MenuBaseAddHotkey(MenuBase* instance, const char* title, unsigned short hotkey, int hotkeyType, bool toggled);
    __declspec(dllexport) ComboBox* MenuBaseAddComboBox(MenuBase* instance, const char* title, const char** items, int itemsCount, int selectedIndex);

    __declspec(dllexport) void MenuBaseRemoveItem(MenuBase* instance, const MenuItem* menuItem);
}
