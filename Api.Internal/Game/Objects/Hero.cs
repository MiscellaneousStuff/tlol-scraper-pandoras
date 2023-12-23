using System.Runtime.InteropServices;
using Api.Game.GameInputs;
using Api.Game.Objects;
using Api.GameProcess;
using Newtonsoft.Json;

namespace Api.Internal.Game.Objects;

[StructLayout(LayoutKind.Sequential)]
internal struct SpellBookInfo
{
    public IntPtr Q { get; set; }
    public IntPtr W { get; set; }
    public IntPtr E { get; set; }
    public IntPtr R { get; set; }
    public IntPtr Summoner1 { get; set; }
    public IntPtr Summoner2 { get; set; }
}

internal class Hero : AiBaseUnit, IHero
{
    public bool IsLocalHero { get; set; } = false;
    public int SpawnCount { get; set; }
    public ISpell AutoAttack { get; set; } = new Spell{SpellSlot = SpellSlot.AutoAttack};
    public ISpell Q { get; set; } = new Spell{SpellSlot = SpellSlot.Q};
    public ISpell W { get; set; } = new Spell{SpellSlot = SpellSlot.W};
    public ISpell E { get; set; } = new Spell{SpellSlot = SpellSlot.E};
    public ISpell R { get; set; } = new Spell{SpellSlot = SpellSlot.R};
    public ISpell Summoner1 { get; set; } = new Spell{SpellSlot = SpellSlot.Summoner1};
    public ISpell Summoner2 { get; set; } = new Spell{SpellSlot = SpellSlot.Summoner2};
    public IActiveCastSpell ActiveCastSpell { get; set; } = new ActiveCastSpell();
    public IAiManager AiManager { get; set; } = new AiManager();
    public IDictionary<string, IBuff> BuffsDictionary { get; set; } = new Dictionary<string, IBuff>();
    public IEnumerable<IBuff> Buffs => BuffsDictionary.Values;
    public bool HasBuff(string name)
    {
        return BuffsDictionary.ContainsKey(name);
    }

    public IBuff? GetBuff(string name)
    {
        if (BuffsDictionary.TryGetValue(name, out var buff))
        {
            return buff;
        }

        return null;
    }

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this, Formatting.Indented);
    }

}