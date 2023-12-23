namespace Api.Game.Offsets;

public interface ISpellOffsets
{
    public OffsetData SpellSlotLevel { get; }
    public OffsetData SpellSlotReadyAt { get; }
    public OffsetData SpellSlotSmiteReadyAt { get; }
    public OffsetData SpellSlotDamage { get; }
    public OffsetData SpellSlotSmiteCharges { get; }
    public OffsetData SpellSlotSpellInput { get; }
    public OffsetData SpellInputStartPosition { get; }
    public OffsetData SpellInputEndPosition { get; }
    public OffsetData SpellInputTargetId { get; }
    OffsetData SpellSlotSpellInfo { get; }
    OffsetData SpellInfoSpellData { get; }
    OffsetData SpellDataSpellName { get; }
    IEnumerable<OffsetData> GetOffsets();
    IEnumerable<OffsetData> GetSpellInputOffsets();
}