namespace Api.Game.Offsets;

public interface IHeroOffsets
{
    OffsetData SpawnCount { get; }
    OffsetData BuffManager { get; }
    OffsetData BuffManagerEntryStart { get; }
    OffsetData BuffManagerEntryEnd { get; }
    OffsetData SpellBook { get; }
    OffsetData AiManager { get; }
    OffsetData ActiveSpell { get; set; }
    IEnumerable<OffsetData> GetOffsets();
}