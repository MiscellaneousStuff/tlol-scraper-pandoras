﻿using Api.Inputs;

namespace Api.Menus;

public interface ISubMenu : IMenuElement
{
    ISubMenu AddSubMenu(string title);
    IToggle AddToggle(string title, bool toggled);
    IValueSlider AddFloatSlider(string title, float value, float minValue, float maxValue, float step, int precision);
    IHotkey AddHotkey(string title, VirtualKey hotkey, HotkeyType hotkeyType, bool toggled);
    IComboBox AddComboBox(string title, string[] items, int selectedIndex);
    IEnumComboBox<T> AddEnumComboBox<T>(string title, T selectedItem) where T : Enum;
}