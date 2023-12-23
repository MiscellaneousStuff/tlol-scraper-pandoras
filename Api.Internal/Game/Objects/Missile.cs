using System.Numerics;
using Api.Game.Data;
using Api.Game.Objects;

namespace Api.Internal.Game.Objects;

internal class Missile : IMissile
{
    public IntPtr Pointer { get; set; }
    public bool RequireFullUpdate { get; set; }
    public bool IsValid { get; set; }
    public int NetworkId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int NameHash { get; set; }
    public float Speed { get; set; }
    public Vector3 Position { get; set; }
    public int SourceIndex { get; set; }
    public int DestinationIndex { get; set; }
    public Vector3 StartPosition { get; set; }
    public Vector3 EndPosition { get; set; }
    public string SpellName { get; set; } = string.Empty;
    public string MissileName { get; set; } = string.Empty;
    public MissileData? MissileData { get; set; }
    public float Width { get; set; }
    public SpellData? SpellData { get; set; }
}