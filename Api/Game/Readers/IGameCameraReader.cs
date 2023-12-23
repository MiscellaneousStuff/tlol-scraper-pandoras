using Api.Game.Objects;

namespace Api.Game.Readers;

public interface IGameCameraReader
{
    bool ReadCamera(IGameCamera? gameCamera);
}