using Api.Game.ObjectTypes;

namespace Api.Game.ObjectNameMappers;

public class MinionNameTypeMap
{
    public List<string> Names { get; set; }
    public MinionType MinionType { get; set; }
}

public interface IMinionNameTypeMapper
{
    MinionType Map(int nameHash);

    MinionType Map(string name);
}