using System.Numerics;

namespace Api.Game.Objects;

public interface IActiveCastSpell
{
    IntPtr Pointer { get; set; }
    bool IsActive { get; set; }
    ActiveSpellType Type { get; set; }
    public int SourceId { get; set; }
    public int TargetId { get; set; }
    Vector3 StartPosition { get; set; }
    Vector3 EndPosition { get; set; }
    float StartTime { get; set; }
    float EndTime { get; set; }
    string Name { get; set; }
}

public enum ActiveSpellType : sbyte
{
    AutoAttack = -1,
    Q = 0,
    W = 1,
    E = 2,
    R = 3,
    Unknown
}
