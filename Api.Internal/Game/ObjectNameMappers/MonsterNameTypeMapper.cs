using Api.Game.ObjectNameMappers;
using Api.Game.ObjectTypes;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Api.Internal.Game.ObjectNameMappers;

internal class MonsterNameTypeMapper : IMonsterNameTypeMapper
{
    private readonly ILogger<MonsterNameTypeMapper> _logger;
    private readonly Dictionary<int, MonsterType> _mappings;
    
    public MonsterNameTypeMapper(ILogger<MonsterNameTypeMapper> logger, JsonSerializerSettings jsonSerializerSettings)
    {
        _logger = logger;
        _mappings = new Dictionary<int, MonsterType>();
        
        var filePath = Path.Combine("Resources", "ObjectNameMappers", $"{nameof(MonsterNameTypeMapper)}.json");
        if (!File.Exists(filePath))
        {
            _logger.LogError($"{nameof(MonsterNameTypeMapper)} config file doesnt exist at path {filePath}");
            return;
        }
        
        var nameMappingsStr = File.ReadAllText(filePath);
        var nameMappings = JsonConvert.DeserializeObject<List<MonsterNameTypeMap>>(nameMappingsStr, jsonSerializerSettings);
        if (nameMappings is null || !nameMappings.Any())
        {
            _logger.LogError($"{nameof(MonsterNameTypeMapper)} config file is empty or doesnt contain proper data.");
            return;
        }
        
        foreach (var nameMap in nameMappings)
        {
            foreach (var name in nameMap.Names)
            {
                _mappings.Add(name.GetHashCode(), nameMap.MonsterType);
            }
        }
    }
    
    public MonsterType Map(int nameHash)
    {
        if (_mappings.TryGetValue(nameHash, out var monsterType))
        {
            return monsterType;
        }

        return MonsterType.Unknown;
    }

    public MonsterType Map(string name)
    {
        return Map(name.GetHashCode());
    }
}