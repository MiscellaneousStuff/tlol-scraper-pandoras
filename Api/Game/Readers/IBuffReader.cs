using Api.Game.Objects;

namespace Api.Game.Readers;

public interface IBuffReader
{
    void ReadBuffs(IDictionary<string, IBuff> buffDictionary, IntPtr start, IntPtr end);
}