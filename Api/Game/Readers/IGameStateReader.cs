using Api.Game.Objects;

namespace Api.Game.Readers;

public interface IGameStateReader
{
    public void ReadGameState(IGameState gameState);
}