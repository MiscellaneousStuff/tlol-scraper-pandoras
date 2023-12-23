using System.Numerics;
using Api.Game.GameInputs;
using Api.GameProcess;

namespace Api.Game.Objects;

public interface IHero : IAiBaseUnit
{
    public bool IsLocalHero { get; set; }
    public int SpawnCount { get; set; }
    ISpell AutoAttack { get; set; }
    ISpell Q { get; set; }
    ISpell W { get; set; }
    ISpell E { get; set; }
    ISpell R { get; set; }
    ISpell Summoner1 { get; set; }
    ISpell Summoner2 { get; set; }
    IActiveCastSpell ActiveCastSpell { get; set; }
    IAiManager AiManager { get; set; }
    IDictionary<string, IBuff> BuffsDictionary { get; set; }
    IEnumerable<IBuff> Buffs { get; }
    public bool HasBuff(string name);
    public IBuff? GetBuff(string name);
}