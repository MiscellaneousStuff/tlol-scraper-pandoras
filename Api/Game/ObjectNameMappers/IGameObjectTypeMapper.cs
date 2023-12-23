using Api.Game.ObjectTypes;

namespace Api.Game.ObjectNameMappers;

public class GameObjectNameTypeMap
{
    public List<string> Names { get; set; }
    public GameObjectType GameObjectType { get; set; }
}

public interface IGameObjectTypeMapper
{
    public GameObjectType Map(int nameHash);
    public GameObjectType Map(string name);
}