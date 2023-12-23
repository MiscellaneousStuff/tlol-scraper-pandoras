using System.Numerics;
using Api.Game.Objects;

namespace Api.Internal.Game.Objects;

internal class ActiveCastSpell : IActiveCastSpell
{
    public IntPtr Pointer { get; set; }
    public bool IsActive { get; set; }
    public ActiveSpellType Type { get; set; }
    public int SourceId { get; set; }
    public int TargetId { get; set; }
    public Vector3 StartPosition { get; set; }
    public Vector3 EndPosition { get; set; }
    public float StartTime { get; set; }
    public float EndTime { get; set; }
    public string Name { get; set; } = string.Empty;
}