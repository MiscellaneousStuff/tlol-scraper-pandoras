using Api.Game.ObjectNameMappers;
using Api.Game.ObjectTypes;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Api.Internal.Game.ObjectNameMappers;

internal class WardNameTypeMapper : IWardNameTypeMapper
{
    private readonly ILogger<WardNameTypeMapper> _logger;
    private readonly Dictionary<int, WardType> _mappings;

    public WardNameTypeMapper(ILogger<WardNameTypeMapper> logger, JsonSerializerSettings jsonSerializerSettings)
    {
        _logger = logger;
        _mappings = new Dictionary<int, WardType>();
        
        var filePath = Path.Combine("Resources", "ObjectNameMappers", $"{nameof(WardNameTypeMapper)}.json");
        if (!File.Exists(filePath))
        {
            _logger.LogError($"{nameof(WardNameTypeMapper)} config file doesnt exist at path {filePath}");
            return;
        }
        
        var nameMappingsStr = File.ReadAllText(filePath);
        var nameMappings = JsonConvert.DeserializeObject<List<WardNameTypeMap>>(nameMappingsStr, jsonSerializerSettings);
        if (nameMappings is null || !nameMappings.Any())
        {
            _logger.LogError($"{nameof(WardNameTypeMapper)} config file is empty or doesnt contain proper data.");
            return;
        }
        
        foreach (var nameMap in nameMappings)
        {
            foreach (var name in nameMap.Names)
            {
                _mappings.Add(name.GetHashCode(), nameMap.WardType);
            }
        }
    }
    
    public WardType Map(int nameHash)
    {        
        if (_mappings.TryGetValue(nameHash, out var wardType))
        {
            return wardType;
        }

        return WardType.Unknown;
    }

    public WardType Map(string name)
    {
        return Map(name.GetHashCode());
    }
}