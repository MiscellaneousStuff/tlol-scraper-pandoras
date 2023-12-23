using Api.Game.Objects;

namespace Api.Game.Readers;

public interface IGameReader
{
    IGameObject? ReadObject(IntPtr address);
    bool ReadObject(IGameObject? gameObject);
}