namespace Api.Menus;

public interface IComboBox : IMenuElement
{
    int SelectedIndex { get; set; }
    string[] Items { get; set; }
    event Action<int> SelectionChanged;
}


public interface IEnumComboBox<T> where T : Enum
{
    T SelectedItem { get; set; }
    event Action<T> SelectionChanged;
}