using Api.Game.ObjectTypes;

namespace Api.Game.ObjectNameMappers;

public class WardNameTypeMap
{
    public List<string> Names { get; set; }
    public WardType WardType { get; set; }
}

public interface IWardNameTypeMapper
{
    WardType Map(int nameHash);

    WardType Map(string name);
}