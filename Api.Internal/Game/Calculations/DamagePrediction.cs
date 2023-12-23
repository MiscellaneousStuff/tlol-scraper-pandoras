using System.Numerics;
using Api.Game.Calculations;
using Api.Game.Managers;
using Api.Game.Objects;
using Api.Internal.Game.Objects;

namespace Api.Internal.Game.Calculations;

public class DamagePrediction : IDamagePrediction
{
    private readonly IDamageCalculator _damageCalculator;
    private readonly IMissileManager _missileManager;
    private readonly IObjectManager _objectManager;
    
    public DamagePrediction(
        IDamageCalculator damageCalculator,
        IMissileManager missileManager,
        IObjectManager objectManager)
    {
        _damageCalculator = damageCalculator;
        _missileManager = missileManager;
        _objectManager = objectManager;
    }

    public float PredictHealth(IAttackableUnit target, float time)
    {
        var missiles = _missileManager.GetMissiles(target.NetworkId).ToList();

        var totalDamage = 0.0f;

        foreach (var missile in missiles)
        {
            totalDamage += GetMissileDamage(missile, target, time);
        }
        
        return target.Health - totalDamage;
    }

    private float GetMissileDamage(IMissile missile, IAttackableUnit target, float time)
    {
        var distance = Vector3.Distance(target.Position, missile.Position);

        var travelTime = distance / missile.Speed;
        if (travelTime > time)
        {
            return 0;
        }
            
        var source = _objectManager.GetByNetworkId(missile.SourceIndex);
        
        if (source == null || source is not AiBaseUnit aiBaseUnit)
        {
            return 0;
        };
        
        return _damageCalculator.GetPhysicalDamage(aiBaseUnit, target, aiBaseUnit.TotalAttackDamage);
    }
}