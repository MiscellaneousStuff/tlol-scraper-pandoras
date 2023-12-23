using Api.Game.Objects;

namespace Api.Game.Calculations;

public enum DamageType
{
    Physical,
    Magic,
    True
}

public interface IDamageCalculator
{
    public float GetDamage(DamageType damageType, IAiBaseUnit source, IAttackableUnit destination, float damage);
    public float GetPhysicalDamage(IAiBaseUnit source, IAttackableUnit destination, float damage);
    public float GetMagicDamage(IAiBaseUnit source, IAttackableUnit destination, float damage);
    public float GetDamage(float damage, float resistance, float flatPenetration, float percentPenetration);
}