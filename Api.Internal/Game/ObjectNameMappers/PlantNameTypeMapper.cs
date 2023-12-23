using Api.Game.ObjectNameMappers;
using Api.Game.ObjectTypes;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Api.Internal.Game.ObjectNameMappers;

public class PlantNameTypeMapper : IPlantNameTypeMapper
{
    private readonly ILogger<PlantNameTypeMapper> _logger;
    private readonly Dictionary<int, PlantType> _mappings;
    
    public PlantNameTypeMapper(ILogger<PlantNameTypeMapper> logger, JsonSerializerSettings jsonSerializerSettings)
    {
        _logger = logger;
        _mappings = new Dictionary<int, PlantType>();
        
        var filePath = Path.Combine("Resources", "ObjectNameMappers", $"{nameof(PlantNameTypeMapper)}.json");
        if (!File.Exists(filePath))
        {
            _logger.LogError($"{nameof(PlantNameTypeMapper)} config file doesnt exist at path {filePath}");
            return;
        }
        
        var nameMappingsStr = File.ReadAllText(filePath);
        var nameMappings = JsonConvert.DeserializeObject<List<PlantNameTypeMap>>(nameMappingsStr, jsonSerializerSettings);
        if (nameMappings is null || !nameMappings.Any())
        {
            _logger.LogError($"{nameof(PlantNameTypeMapper)} config file is empty or doesnt contain proper data.");
            return;
        }
        
        foreach (var nameMap in nameMappings)
        {
            foreach (var name in nameMap.Names)
            {
                _mappings.Add(name.GetHashCode(), nameMap.PlantType);
            }
        }
    }
    
    
    public PlantType Map(int nameHash)
    {
        if (_mappings.TryGetValue(nameHash, out var minionType))
        {
            return minionType;
        }

        return PlantType.Unknown;
    }

    public PlantType Map(string name)
    {
        return Map(name.GetHashCode());
    }
}