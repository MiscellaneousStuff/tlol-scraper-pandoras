namespace Api.Settings;

public interface ISettingsProvider
{
    void SetValue<T>(string key, T value);
    bool ReadValue<T>(string key, out T? value);
    void Load();
    void Save();
    void Load(string fileName);
    void Save(string fileName);
}