using Api.Game.Objects;
using Api.Game.Readers;
using Api.Internal.Game.Objects;

namespace Api.Internal.Game.Readers;

internal class GameReader : IGameReader
{
    private readonly IGameObjectReader _gameObjectReader;

    internal GameReader(IGameObjectReader gameObjectReader)
    {
        _gameObjectReader = gameObjectReader;
    }
    
    public IGameObject? ReadObject(IntPtr address)
    {
        var gameObject = new GameObject
        {
            RequireFullUpdate = true
        };

        _gameObjectReader.ReadObject(gameObject);
        
        return gameObject;
    }

    public bool ReadObject(IGameObject? gameObject)
    {
        return _gameObjectReader.ReadObject(gameObject);
    }
}