using Api.Game.ObjectTypes;

namespace Api.Game.Objects;

public interface IMonster : IAiBaseUnit
{
    public MonsterType MonsterType { get; set; }
}