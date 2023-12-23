using Api;
using Api.Game.Managers;
using Api.Game.Objects;
using Api.Game.ObjectTypes;
using Api.Menus;
using Api.Scripts;

// For saving JSON data
using System;
using System.IO;
using Newtonsoft.Json;
using Api.Game.Offsets;
using System;
using System.Diagnostics;
using Api.Internal.Game.Managers;

namespace Scripts.CSharpScripts.Utility;

public class Extractor : IScript
{
    public string Name => "Extractor";
    public ScriptType ScriptType => ScriptType.Utility;
    public bool Enabled { get; set; }
    private readonly ITurretManager _turretManager;
    private readonly IHeroManager _heroManager;
    private readonly IRenderer _renderer;
    private readonly IGameState _gameState;
    private readonly ILocalPlayer _localPlayer;
    private readonly IGameCamera _gameCamera;
    private readonly IObjectManager _objectManager;
    private readonly IMissileManager _missileManager;
    private readonly string[] _commandLineArgs;
    public List<Object> observations;
    public int _replayMult;
    public string _replayFile;
    public string _replayCmd;

    public float killTime;

    public Extractor(
        IMainMenu mainMenu,
        ITurretManager turretManager,
        IHeroManager heroManager,
        IRenderer renderer,
        IGameState gameState,
        ILocalPlayer localPlayer,
        IGameCamera gameCamera,
        IObjectManager objectManager,
        IMissileManager missileManager,
        string[] args)
    {
        _turretManager = turretManager;
        _heroManager = heroManager;
        _renderer = renderer;
        _gameState = gameState;
        _localPlayer = localPlayer;
        _gameCamera = gameCamera;
        _objectManager = objectManager;
        _missileManager = missileManager;
        _commandLineArgs = args;

        observations = new List<Object>();

        killTime = 60.0f;
    }
    
    public void OnLoad()
    {
        _replayMult = 1; // Default value if not provided
        _replayFile = string.Empty;
        _replayCmd = string.Empty;

        // Get Replay Speed and Replay Output File
        foreach (var arg in _commandLineArgs)
        {
            var splitArg = arg.Split('=');
            if (splitArg.Length == 2)
            {
                string key = splitArg[0].ToLower(); // Convert key to lower case for case-insensitive comparison
                var value = splitArg[1];

                switch (key)
                {
                    case "replay_mult":
                        if (int.TryParse(value, out var speed))
                        {
                            _replayMult = speed;
                        }
                        else
                        {
                            Console.WriteLine($"Invalid replay_speed: {value}");
                        }
                        break;
                    case "replay_file":
                        _replayFile = value;
                        break;
                    case "replay_cmd":
                        _replayCmd = value;
                        break;
                    case "end_time":
                        if (float.TryParse(value, out var end_time))
                        {
                            killTime = end_time;
                        }
                        else
                        {
                            Console.WriteLine($"Invalid end_time: {value}");
                        }
                        break;
                    default:
                        Console.WriteLine($"Unknown argument: {key}");
                        break;
                }
            }
            else
            {
                Console.WriteLine($"Invalid argument format: {arg}");
            }
        }
        Console.WriteLine($"Replay Mult: {_replayMult}");
        Console.WriteLine($"Replay File: {_replayFile}");

        // Execute replay speed setting
        string cmd = $"\"{_replayCmd} {_replayMult}\"";
        Console.WriteLine(cmd);
        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = "cmd.exe",
            Arguments = $"/c {cmd}",
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };
        Process process = new Process
        {
            StartInfo = startInfo
        };
        process.Start();
        process.WaitForExit();
    }

    public void OnUnload()
    {
        
    }

    public void OnUpdate(float deltaTime)
    {
        float tm = _gameState.Time;

        if (tm > 0.0 && tm <= killTime) {
            Console.WriteLine("Game Time: {0} {1}", tm, deltaTime);
            List<Object> heros   = new List<Object>();
            List<Object> minions = new List<Object>();
            List<Object> monsters = new List<Object>();
            List<Object> turrets = new List<Object>();
            List<Object> missiles = new List<Object>();

            // Add champs
            foreach (IHero hero in _heroManager.GetHeroes()) {
                var champ = new {
                    name = hero.ObjectName,
                    net_id = hero.NetworkId,
                    
                    hp = hero.Health,
                    max_hp = hero.MaxHealth,
                    mana = hero.Mana,
                    max_mana = hero.MaxMana,

                    armor = hero.TotalArmor,
                    mr = hero.TotalMagicResistance,

                    ad = hero.TotalAttackDamage,
                    ap = hero.AbilityPower,

                    level = hero.Level,
                    atk_range = hero.AttackRange,

                    visible = hero.IsVisible,
                    team = hero.Team,

                    pos_x = hero.Position.X,
                    pos_z = hero.Position.Z,

                    auto_name = hero.AutoAttack.Name,
                    auto_cd = hero.AutoAttack.Cooldown,

                    q_name = hero.Q.Name,
                    q_cd = hero.Q.Cooldown,
                    w_name = hero.W.Name,
                    w_cd = hero.W.Cooldown,
                    e_name = hero.E.Name,
                    e_cd = hero.E.Cooldown,
                    r_name = hero.R.Name,
                    r_cd = hero.R.Cooldown,
                    d_name = hero.Summoner1.Name,
                    d_cd = hero.Summoner1.Cooldown,
                    f_name = hero.Summoner2.Name,
                    f_cd = hero.Summoner2.Cooldown
                };
                heros.Add(champ);
            }

            // Add minions
            foreach (IMinion m_minion in _objectManager.MinionManager.GetAllyMinions()) {
                var minion = new {
                    name = m_minion.ObjectName,
                    
                    hp = m_minion.Health,
                    max_hp = m_minion.MaxHealth,
                    mana = m_minion.Mana,
                    max_mana = m_minion.MaxMana,

                    armor = m_minion.TotalArmor,
                    mr = m_minion.TotalMagicResistance,

                    ad = m_minion.TotalAttackDamage,
                    ap = m_minion.AbilityPower,

                    level = m_minion.Level,
                    atk_range = m_minion.AttackRange,

                    visible = m_minion.IsVisible,
                    team = m_minion.Team,

                    pos_x = m_minion.Position.X,
                    pos_z = m_minion.Position.Z
                };
                minions.Add(minion);
            }
            foreach (IMinion m_minion in _objectManager.MinionManager.GetEnemyMinions()) {
                var minion = new {
                    name = m_minion.ObjectName,
                    
                    hp = m_minion.Health,
                    max_hp = m_minion.MaxHealth,
                    mana = m_minion.Mana,
                    max_mana = m_minion.MaxMana,

                    armor = m_minion.TotalArmor,
                    mr = m_minion.TotalMagicResistance,

                    ad = m_minion.TotalAttackDamage,
                    ap = m_minion.AbilityPower,

                    level = m_minion.Level,
                    atk_range = m_minion.AttackRange,

                    visible = m_minion.IsVisible,
                    team = m_minion.Team,

                    pos_x = m_minion.Position.X,
                    pos_z = m_minion.Position.Z
                };
                minions.Add(minion);
            }

            // Add monsters
            foreach (IMonster m_monster in _objectManager.MonsterManager.GetMonsters()) {
                var monster = new {
                    name = m_monster.ObjectName,
                    
                    hp = m_monster.Health,
                    max_hp = m_monster.MaxHealth,
                    mana = m_monster.Mana,
                    max_mana = m_monster.MaxMana,

                    armor = m_monster.TotalArmor,
                    mr = m_monster.TotalMagicResistance,

                    ad = m_monster.TotalAttackDamage,
                    ap = m_monster.AbilityPower,

                    level = m_monster.Level,
                    atk_range = m_monster.AttackRange,

                    visible = m_monster.IsVisible,
                    team = m_monster.Team,

                    pos_x = m_monster.Position.X,
                    pos_z = m_monster.Position.Z
                };
                monsters.Add(monster);
            }
            
            // Add turrets
            foreach (ITurret t_turret in _turretManager.GetAllyTurrets()) {
                var turret = new {
                    name = t_turret.ObjectName,
                    
                    hp = t_turret.Health,
                    max_hp = t_turret.MaxHealth,
                    mana = t_turret.Mana,
                    max_mana = t_turret.MaxMana,

                    armor = t_turret.TotalArmor,
                    mr = t_turret.TotalMagicResistance,

                    ad = t_turret.TotalAttackDamage,
                    ap = t_turret.AbilityPower,

                    level = t_turret.Level,
                    atk_range = t_turret.AttackRange,

                    visible = t_turret.IsVisible,
                    team = t_turret.Team,

                    pos_x = t_turret.Position.X,
                    pos_z = t_turret.Position.Z
                };
                turrets.Add(turret);
            }
            foreach (ITurret t_turret in _turretManager.GetEnemyTurrets()) {
                var turret = new {
                    name = t_turret.ObjectName,
                    
                    hp = t_turret.Health,
                    max_hp = t_turret.MaxHealth,
                    mana = t_turret.Mana,
                    max_mana = t_turret.MaxMana,

                    armor = t_turret.TotalArmor,
                    mr = t_turret.TotalMagicResistance,

                    ad = t_turret.TotalAttackDamage,
                    ap = t_turret.AbilityPower,

                    level = t_turret.Level,
                    atk_range = t_turret.AttackRange,

                    visible = t_turret.IsVisible,
                    team = t_turret.Team,

                    pos_x = t_turret.Position.X,
                    pos_z = t_turret.Position.Z
                };
                turrets.Add(turret);
            }

            // Add missiles
            foreach (IMissile m_missile in _missileManager.GetMissiles()) {
                var missile = new {
                    name = m_missile.Name,
                    missile_name = m_missile.MissileName,
                    spell_name = m_missile.SpellName,

                    src_idx = m_missile.SourceIndex,
                    dst_idx = m_missile.DestinationIndex,

                    start_pos_x = m_missile.StartPosition.X,
                    start_pos_z = m_missile.StartPosition.Z,
                    end_pos_x = m_missile.EndPosition.X,
                    end_pos_z = m_missile.EndPosition.Z,

                    pos_x = m_missile.Position.X,
                    pos_z = m_missile.Position.Z
                };
                missiles.Add(missile);
            }

            var observation = new {
                time    = tm,
                champs  = heros,
                turrets = turrets,
                minions = minions,
                monsters = monsters,
                missiles = missiles,
            };
            observations.Add(observation);
        } else {
            Console.WriteLine("WRITING TO OUTPUT Game Time: {0}, Replay File: {1}", tm, _replayFile);
            string jsonString = JsonConvert.SerializeObject(observations, Formatting.Indented);
            File.WriteAllText(_replayFile, jsonString);
            Console.WriteLine("WRITTEN OUTPUT!: {0}", _replayFile);
            Environment.Exit(0);
        }
    }

    public void OnRender(float deltaTime)
    {
        
    }
}