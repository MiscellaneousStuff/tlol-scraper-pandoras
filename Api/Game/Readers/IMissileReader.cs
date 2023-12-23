using Api.Game.Objects;

namespace Api.Game.Readers;

public interface IMissileReader : IDisposable
{    
    bool ReadMissile(IMissile? missile);
}