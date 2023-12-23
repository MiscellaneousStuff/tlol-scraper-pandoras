using Api.Game.Objects;

namespace Api.Game.Managers;

public interface IMissileManager : IManager
{
    IEnumerable<IMissile> GetMissiles();
    IEnumerable<IMissile> GetMissiles(int networkId);
}