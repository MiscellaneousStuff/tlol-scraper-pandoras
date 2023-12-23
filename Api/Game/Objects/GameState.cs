namespace Api.Game.Objects;

public interface IGameState
{
    float Time { get; set; }
    bool IsGameActive => Time > 1;
}