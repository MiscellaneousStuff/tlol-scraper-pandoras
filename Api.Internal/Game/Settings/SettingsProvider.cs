using Api.Settings;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Api.Internal.Game.Settings;

internal class SettingsProvider : ISettingsProvider
{
    private static readonly string FilePath = Path.Combine("Resources", "Settings");
    private const string FileName = "settings.json";
    private JObject _settings;

    public SettingsProvider()
    {
        Load();
    }
    
    public void SetValue<T>(string key, T value)
    {
        if (value != null)
        {
            _settings[key] = JToken.FromObject(value);
        }
        else
        {
            _settings[key] = default;
        }
    }

    public bool ReadValue<T>(string key, out T? value)
    {
        if (_settings.TryGetValue(key, out var token))
        {
            value = token.ToObject<T>();
            return true;
        }

        value = default;
        return false;
    }

    public void Load()
    {
        Load(FileName);
    }

    public void Save()
    {
        Save(FileName);
    }

    public void Load(string fileName)
    {
        if (!Directory.Exists(FilePath))
        {
            Directory.CreateDirectory(FilePath);
        }
        
        var filePath = Path.Combine(FilePath, fileName);
        if (File.Exists(filePath))
        {
            var json = File.ReadAllText(filePath);
            _settings = JObject.Parse(json);
        }
        else
        {
            _settings = new JObject();
        }
    }

    public void Save(string fileName)
    {
        if (!Directory.Exists(FilePath))
        {
            Directory.CreateDirectory(FilePath);
        }
        
        var filePath = Path.Combine(FilePath, fileName);
        var json = _settings.ToString(Formatting.Indented);
        File.WriteAllText(filePath, json);
    }
}