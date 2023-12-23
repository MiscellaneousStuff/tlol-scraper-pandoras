using Api.Game.ObjectNameMappers;
using Api.Game.ObjectTypes;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Api.Internal.Game.ObjectNameMappers;

internal class MinionNameTypeMapper : IMinionNameTypeMapper
{
    private readonly ILogger<MinionNameTypeMapper> _logger;
    private readonly Dictionary<int, MinionType> _mappings;

    public MinionNameTypeMapper(ILogger<MinionNameTypeMapper> logger, JsonSerializerSettings jsonSerializerSettings)
    {
        _logger = logger;
        _mappings = new Dictionary<int, MinionType>();
        
        var filePath = Path.Combine("Resources", "ObjectNameMappers", $"{nameof(MinionNameTypeMapper)}.json");
        if (!File.Exists(filePath))
        {
            _logger.LogError($"{nameof(MinionNameTypeMapper)} config file doesnt exist at path {filePath}");
            return;
        }
        
        var nameMappingsStr = File.ReadAllText(filePath);
        var nameMappings = JsonConvert.DeserializeObject<List<MinionNameTypeMap>>(nameMappingsStr, jsonSerializerSettings);
        if (nameMappings is null || !nameMappings.Any())
        {
            _logger.LogError($"{nameof(MinionNameTypeMapper)} config file is empty or doesnt contain proper data.");
            return;
        }
        
        foreach (var nameMap in nameMappings)
        {
            foreach (var name in nameMap.Names)
            {
                _mappings.Add(name.GetHashCode(), nameMap.MinionType);
            }
        }
    }
    
    public MinionType Map(int nameHash)
    {
        if (_mappings.TryGetValue(nameHash, out var minionType))
        {
            return minionType;
        }

        return MinionType.Unknown;
    }

    public MinionType Map(string name)
    {
        return Map(name.GetHashCode());
    }
}