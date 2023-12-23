using Api.Game.GameInputs;
using Api.Game.Objects;

namespace Api.Game.Managers;

public interface IGameManager : IManager
{
    IGameState GameState { get; }
    ILocalPlayer LocalPlayer { get; }
    IGameInput GameInput { get; }
    IGameCamera GameCamera { get; }
    IHeroManager HeroManager { get; }
    IObjectManager ObjectManager { get; }
    ITurretManager TurretManager { get; }
    IInhibitorManager InhibitorManager { get; }
    IMissileManager MissileManager { get; }
    event Action GameLoaded;
    void Unload();
}