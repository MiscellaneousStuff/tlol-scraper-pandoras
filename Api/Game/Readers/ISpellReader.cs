using Api.Game.Objects;

namespace Api.Game.Readers;

public interface ISpellReader
{
    bool ReadSpell(ISpell spell, IntPtr spellPointer);
}