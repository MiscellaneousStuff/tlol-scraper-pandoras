using Api.Game.ObjectTypes;

namespace Api.Game.Objects;

public interface IMinion : IAiBaseUnit
{
    public MinionType MinionType { get; set; }
}