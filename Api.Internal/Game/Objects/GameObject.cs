using System.Numerics;
using Api.Game.Objects;
using Api.Game.ObjectTypes;
using Newtonsoft.Json;

namespace Api.Internal.Game.Objects;

internal class GameObject : IGameObject
{
    private IntPtr _pointer;
    public IntPtr Pointer 
    {
        get => _pointer;
        set
        {
            if (_pointer == value) return;
            
            RequireFullUpdate = true;
            _pointer = value;
        } 
    }

    [JsonIgnore]
    public bool RequireFullUpdate { get; set; } = true;
    public int Index { get; set; }
    public Vector3 Position { get; set;}
    public int Team { get; set;}
    public int NetworkId { get; set;}
    public bool IsVisible { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ObjectName { get; set; } = string.Empty;
    public int ObjectNameHash { get; set; }
    public bool IsValid { get; set;}
    public GameObjectType GameObjectType { get; set; }

    public bool IsEnemy(IGameObject gameObject)
    {
        return gameObject.Team != Team;
    }

    public IGameObject AsGameObject()
    {
        return this as IGameObject;
    }

    public float Distance(IGameObject gameObject)
    {
        return Distance(gameObject.Position);
    }

    public float Distance(Vector3 position)
    {
        return Vector3.Distance(position, Position);
    }

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this, Formatting.Indented);
    }
}