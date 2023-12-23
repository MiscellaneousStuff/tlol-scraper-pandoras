using Api.Game.ObjectTypes;

namespace Api.Game.ObjectNameMappers;

public class MonsterNameTypeMap
{
    public List<string> Names { get; set; }
    public MonsterType MonsterType { get; set; }
}

public interface IMonsterNameTypeMapper
{
    MonsterType Map(int nameHash);
    MonsterType Map(string name);
}