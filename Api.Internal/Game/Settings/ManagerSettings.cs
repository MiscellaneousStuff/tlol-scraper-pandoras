using Api.Settings;

namespace Api.Internal.Game.Settings;

internal class ManagerSettings : IManagerSettings
{
    private const string FilePath = "settings.json";
    public float MinionManagerCacheTime { get; } = 0;
    public float HeroCacheTime { get; } = 0;
    public float MissileCacheTime { get; } = 0;
    public float TurretsCacheTime { get; } = 0;
    public float InhibitorsCacheTime { get; } = 0;
    
    public void Load()
    {
    }

    public void Save()
    {
    }

    public ManagerSettings()
    {
        
    }
}