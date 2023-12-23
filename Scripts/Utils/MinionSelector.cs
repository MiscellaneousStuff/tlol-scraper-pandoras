using System.Numerics;
using Api.Game.Calculations;
using Api.Game.Managers;
using Api.Game.Objects;

namespace Scripts.Utils;

public struct MinionPrediction
{
    public IMinion? Minion { get; set; }
    public float PredictedHealth { get; set; }
    public bool IsValid => Minion is not null;
}

public interface IMinionSelector
{
    IEnumerable<MinionPrediction> GetKillableMinions(float range);
    IEnumerable<MinionPrediction> GetKillableMinions(IEnumerable<IMinion> minions);
    IEnumerable<MinionPrediction> GetKillableMinions(float range, float damage, DamageType damageType);
    IEnumerable<MinionPrediction> GetKillableMinions(IEnumerable<IMinion> minions, float damage, DamageType damageType);
    
    MinionPrediction GetBestKillableMinion(float range);
    
    MinionPrediction GetBestKillableMinion(float range, float damage, DamageType damageType);

    IMinion? GetHealthiestMinion(float range);
}

internal class MinionSelector : IMinionSelector
{
    private readonly ILocalPlayer _localPlayer;
    private readonly IDamagePrediction _damagePrediction;
    private readonly IMinionManager _minionManager;
    private readonly IDamageCalculator _damageCalculator;

    public MinionSelector(
        ILocalPlayer localPlayer,
        IDamagePrediction damagePrediction,
        IMinionManager minionManager,
        IDamageCalculator damageCalculator)
    {
        _localPlayer = localPlayer;
        _damagePrediction = damagePrediction;
        _minionManager = minionManager;
        _damageCalculator = damageCalculator;
    }

    public IEnumerable<MinionPrediction> GetKillableMinions(float range)
    {
        return GetKillableMinions(_minionManager.GetEnemyMinions(range));
    }

    public IEnumerable<MinionPrediction> GetKillableMinions(IEnumerable<IMinion> minions)
    {
        var damage = _localPlayer.TotalAttackDamage;
        return GetKillableMinions(minions, damage, DamageType.Physical);
    }

    public IEnumerable<MinionPrediction> GetKillableMinions(float range, float damage, DamageType damageType)
    {
        return GetKillableMinions(_minionManager.GetEnemyMinions(range), damage, damageType);
    }

    public IEnumerable<MinionPrediction> GetKillableMinions(IEnumerable<IMinion> minions, float damage, DamageType damageType)
    {
        foreach (var minion in minions)
        {
            var timeAttackArrives = GetTimeToAttack(minion);
            var predictedHealth = _damagePrediction.PredictHealth(minion, timeAttackArrives);
            var damageToMinion = _damageCalculator.GetDamage(damageType, _localPlayer, minion, damage);
            if (predictedHealth <= damageToMinion)
            {
                yield return new MinionPrediction
                {
                    PredictedHealth = predictedHealth,
                    Minion = minion
                };
            }
        }
    }

    public MinionPrediction GetBestKillableMinion(float range)
    {
        return GetBestKillableMinion(range, _localPlayer.TotalAttackDamage, DamageType.Physical);
    }

    public MinionPrediction GetBestKillableMinion(float range, float damage, DamageType damageType)
    {
        var minHealth = float.MaxValue;
        IMinion? target = null;

        foreach (var killableMinion in GetKillableMinions(range, damage, damageType))
        {
            if (minHealth > killableMinion.PredictedHealth)
            {
                minHealth = killableMinion.PredictedHealth;
                target = killableMinion.Minion;
            }
        }

        return new MinionPrediction
        {
            Minion = target,
            PredictedHealth = minHealth,
        };
    }

    public IMinion? GetHealthiestMinion(float range)
    {
        var maxHealth = float.MinValue;
        IMinion? target = null;
        var damage = _localPlayer.TotalAttackDamage;
        foreach (var minion in _minionManager.GetEnemyMinions(range))
        {
            var timeAttackArrives = GetTimeToAttack(minion);
            var damageToMinion = _damageCalculator.GetPhysicalDamage(_localPlayer, minion, damage);
            var predictedHealth = _damagePrediction.PredictHealth(minion, timeAttackArrives) - damageToMinion;
            if (predictedHealth > maxHealth)
            {
                maxHealth = predictedHealth;
                target = minion;
            }
        }

        return target;
    }

    private float GetTimeToAttack(IGameObject gameObject)
    {
        if (_localPlayer.UnitData is not null && _localPlayer.UnitData.MissileData is not null)
        {
            return gameObject.Distance(_localPlayer) / _localPlayer.UnitData.MissileData.Speed;
        }

        return 0.001f;
    }
}