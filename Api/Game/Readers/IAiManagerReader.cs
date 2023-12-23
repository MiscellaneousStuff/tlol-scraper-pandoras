using Api.Game.Objects;

namespace Api.Game.Readers;

public interface IAiManagerReader
{
    void ReadAiManager(IHero hero, IntPtr aiManagerPointer);
}