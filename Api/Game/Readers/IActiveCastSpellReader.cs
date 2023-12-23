using Api.Game.Objects;

namespace Api.Game.Readers;

public interface IActiveCastSpellReader
{
    bool ReadSpell(IActiveCastSpell spell, IntPtr spellPointer);
}