using Api.Game.ObjectTypes;

namespace Api.Game.Objects;

public interface IPlant : IAttackableUnit
{
    public PlantType PlantType { get; set; }
}