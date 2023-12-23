namespace Api.Game.Offsets;

public interface IActiveCastSpellOffsets
{
    OffsetData Type { get; set; }
    OffsetData SourceId { get; set; }
    OffsetData TargetId { get; set; }
    OffsetData StartPosition { get; set; }
    OffsetData EndPosition { get; set; }
    OffsetData StartTime { get; set; }
    OffsetData EndTime { get; set; }
    OffsetData SpellInfo { get; set; }
    OffsetData SpellInfoName { get; set; }
    IEnumerable<OffsetData> GetOffsets();
}