namespace Api.Game.Offsets;

public interface IMissileOffsets
{
    OffsetData NetworkId { get; set; }
    public OffsetData Name { get; }
    public OffsetData Speed { get; }
    public OffsetData Position { get; }
    public OffsetData SourceIndex { get; }
    public OffsetData DestinationIndex { get; }
    public OffsetData StartPosition { get; }
    public OffsetData EndPosition { get; }
    public OffsetData SpellInfo { get; }
    public OffsetData SpellInfoSpellName { get; }
    public OffsetData SpellInfoMissileName { get; }
    IEnumerable<OffsetData> GetOffsets();
    IEnumerable<OffsetData> GetSpellInfoOffsets();
}