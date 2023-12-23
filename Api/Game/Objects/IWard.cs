using Api.Game.ObjectTypes;

namespace Api.Game.Objects;

public interface IWard : IAttackableUnit
{
    public WardType WardType { get; set; }
}