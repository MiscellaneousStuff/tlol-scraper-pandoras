namespace Api.Settings;

public interface IManagerSettings
{
    public float MinionManagerCacheTime { get; }
    public float HeroCacheTime { get; }
    public float MissileCacheTime { get; }
    public float TurretsCacheTime { get; }
    public float InhibitorsCacheTime { get; }
    void Load();
    void Save();
}