using System.Numerics;
using Api.Game.Data;
using Api.Game.GameInputs;
using Api.Game.Objects;

namespace Api.Internal.Game.Objects;

internal class SpellInput : ISpellInput
{
    public IntPtr Pointer { get; set; }
    public Vector3 SpellInputStartPosition { get; set; } = Vector3.Zero;
    public Vector3 SpellInputEndPosition { get; set; } = Vector3.Zero;
    public int SpellInputTargetId { get; set; } = 0;
}

internal class Spell : ISpell
{
    public IntPtr Pointer { get; set; }
    public SpellSlot SpellSlot { get; set; }
    public string Name { get; set; } = string.Empty;
    public int NameHash { get; set; }
    public int Level { get; set; }
    public float Cooldown { get; set; }
    public float SmiteCooldown { get; set; }
    public float TotalCooldown { get; set; }
    public float Damage { get; set; }
    public float ManaCost { get; set; }
    public bool IsReady { get; set; }
    public int Stacks { get; set; }
    public ISpellInput SpellInput { get; set; } = new SpellInput();
    public bool SmiteIsReady { get; set; }
    public SpellData? SpellData { get; set; }
    public float Range { get; set; }
}