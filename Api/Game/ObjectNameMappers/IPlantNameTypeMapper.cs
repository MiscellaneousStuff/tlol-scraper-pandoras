using Api.Game.ObjectTypes;

namespace Api.Game.ObjectNameMappers;

public class PlantNameTypeMap
{
    public List<string> Names { get; set; }
    public PlantType PlantType { get; set; }
}

public interface IPlantNameTypeMapper
{
    PlantType Map(int nameHash);

    PlantType Map(string name);
}