using System.Numerics;
using Api.Game.Data;
using Api.Game.GameInputs;

namespace Api.Game.Objects;

public interface ISpellInput
{
    IntPtr Pointer { get; set; }
    Vector3 SpellInputStartPosition { get; set; }
    Vector3 SpellInputEndPosition { get; set; }
    int SpellInputTargetId { get; set; }
}

public interface ISpell
{
    IntPtr Pointer { get; set; }
    SpellSlot SpellSlot { get; set; }
    string Name { get; set; }
    int NameHash { get; set; }
    int Level { get; set; }

    float Cooldown { get; set; }
    float SmiteCooldown { get; set; }
    float Damage { get; set; }
    float ManaCost { get; set; }
    bool IsReady { get; set; }
    int Stacks { get; set; }
    ISpellInput SpellInput { get; set; }
    bool SmiteIsReady { get; set; }
    SpellData? SpellData { get; set; }
    float Range { get; set; }
}