using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Api.Game.ObjectTypes;

namespace Api.Game.Objects
{
    public interface IGameObject : IBaseObject
    {
        int Team { get; set; }
        Vector3 Position { get; set; }
        bool IsVisible { get; set; }
        string Name { get; set; }
        string ObjectName { get; set; }
        public int ObjectNameHash { get; set; }
        GameObjectType GameObjectType { get; set; }
        bool IsEnemy(IGameObject gameObject);
        IGameObject AsGameObject();
        float Distance(IGameObject gameObject);
        float Distance(Vector3 position);
    }
}
