using System.Numerics;
using Api.Game.Data;

namespace Api.Game.Objects;

public interface IMissile : IBaseObject
{
    public string Name { get; set; }
    public int NameHash { get; set; }
    public float Speed { get; set; }
    public Vector3 Position { get; set; }
    public int SourceIndex { get; set; }
    public int DestinationIndex { get; set; }
    public Vector3 StartPosition { get; set; }
    public Vector3 EndPosition { get; set; }
    public string SpellName { get; set; }
    public string MissileName { get; set; }
    MissileData? MissileData { get; set; }
    float Width { get; set; }
    SpellData? SpellData { get; set; }
}