using Api.Game.Objects;

namespace Scripts.Utils;

public class Timer
{
    private float _targetTime = 0;
    private readonly IGameState _gameState;
    public bool IsReady => _gameState.Time >= _targetTime;
    
    public Timer(IGameState gameState)
    {
        _gameState = gameState;
        _targetTime = 0;
    }

    public void SetTime(float time)
    {
        _targetTime = time;
    }     
    
    public void SetDelay(float delay)
    {
        _targetTime = _gameState.Time + delay;
    } 
}