﻿using Api.Game.GameInputs;
using Api.Game.Managers;
using Api.Game.Objects;
using Api.Game.Offsets;
using Api.Game.Readers;
using Api.GameProcess;
using Api.Internal.Game.Objects;

namespace Api.Internal.Game.Managers;

internal class GameManager : IGameManager
{
    private bool _gameLoaded = false;
    private readonly ITargetProcess _targetProcess;
    private readonly IBaseOffsets _baseOffsets;

    private readonly IHeroReader _heroReader;
    private readonly IGameStateReader _gameStateReader;
    private readonly IGameCameraReader _gameCameraReader;

    public IGameState GameState { get; }
    public ILocalPlayer LocalPlayer { get; }
    public IGameInput GameInput { get; }
    public IGameCamera GameCamera { get; }
    public IHeroManager HeroManager { get; }
    public IObjectManager ObjectManager { get; }
    public ITurretManager TurretManager { get; }
    public IInhibitorManager InhibitorManager { get; }
    public IMissileManager MissileManager { get; }
    public event Action? GameLoaded;

    public bool IsGameActive => _gameLoaded;
    
    public GameManager(
        ITargetProcess targetProcess,
        IBaseOffsets baseOffsets,
        IGameState gameState,
        IGameStateReader gameStateReader,
        IObjectManager objectManager,
        IGameCameraReader gameCameraReader, 
        IGameInput gameInput,
        IGameCamera gameCamera,
        IHeroReader heroReader,
        ILocalPlayer localPlayer,
        ITurretManager turretManager,
        IInhibitorManager inhibitorManager,
        IMissileManager missileManager,
        IHeroManager heroManager)
    {
        _targetProcess = targetProcess;
        _baseOffsets = baseOffsets;
        GameState = gameState;
        _gameStateReader = gameStateReader;
        ObjectManager = objectManager;
        _gameCameraReader = gameCameraReader;
        GameInput = gameInput;
        GameCamera = gameCamera;
        _heroReader = heroReader;
        LocalPlayer = localPlayer;
        TurretManager = turretManager;
        InhibitorManager = inhibitorManager;
        MissileManager = missileManager;
        HeroManager = heroManager;
    }

    public void Update(float deltaTime)
    {
        _gameStateReader.ReadGameState(GameState);
        if (!GameState.IsGameActive)
        {
            return;
        }
        
        GameInput.Update(deltaTime);
        _gameCameraReader.ReadCamera(GameCamera);
        
        UpdateLocalPlayer();
        
        HeroManager.Update(deltaTime);
        TurretManager.Update(deltaTime);
        InhibitorManager.Update(deltaTime);
        ObjectManager.Update(deltaTime);
        MissileManager.Update(deltaTime);

        if (!_gameLoaded)
        {
            GameLoaded?.Invoke();
            _gameLoaded = true;
        }
    }

    private void UpdateLocalPlayer()
    {
        if (_targetProcess.ReadModulePointer(_baseOffsets.LocalPlayer, out var localPlayerPointer))
        {
            LocalPlayer.Pointer = localPlayerPointer;
            LocalPlayer.IsLocalHero = true;
            //Console.WriteLine(LocalPlayer.Pointer.ToString("X"));
            _heroReader.ReadHero(LocalPlayer);
        }
        else
        {
            LocalPlayer.IsValid = false;
        }
    }

    public void Dispose()
    {
        ObjectManager.Dispose();
        _gameLoaded = false;
    }
    
    public void Unload()
    {
        _gameLoaded = false;
    }
}