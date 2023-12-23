using Api.Game.ObjectNameMappers;
using Api.Game.ObjectTypes;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Api.Internal.Game.ObjectNameMappers;

internal class GameObjectTypeMapper : IGameObjectTypeMapper
{
    private readonly ILogger<GameObjectTypeMapper> _logger;
    private readonly Dictionary<int, GameObjectType> _mappings;
    
    public GameObjectTypeMapper(ILogger<GameObjectTypeMapper> logger, JsonSerializerSettings jsonSerializerSettings)
    {
        _logger = logger;
        _mappings = new Dictionary<int, GameObjectType>();
        
        var filePath = Path.Combine("Resources", "ObjectNameMappers", $"{nameof(GameObjectTypeMapper)}.json");
        if (!File.Exists(filePath))
        {
            _logger.LogError($"{nameof(GameObjectTypeMapper)} config file doesnt exist at path {filePath}");
            return;
        }
        
        var nameMappingsStr = File.ReadAllText(filePath);
        var nameMappings = JsonConvert.DeserializeObject<List<GameObjectNameTypeMap>>(nameMappingsStr, jsonSerializerSettings);
        if (nameMappings is null || !nameMappings.Any())
        {
            _logger.LogError($"{nameof(GameObjectTypeMapper)} config file is empty or doesnt contain proper data.");
            return;
        }
        
        foreach (var nameMap in nameMappings)
        {
            foreach (var name in nameMap.Names)
            {
                _mappings.Add(name.GetHashCode(), nameMap.GameObjectType);
            }
        }
    }
    
    public GameObjectType Map(int nameHash)
    {
        if (_mappings.TryGetValue(nameHash, out var gameObjectType))
        {
            return gameObjectType;
        }

        return GameObjectType.Unknown;
    }

    public GameObjectType Map(string name)
    {
        return Map(name.GetHashCode());
    }
}