using Api.Game.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Api.Game.Managers
{
    public struct ObjectCreateResult
    {
        public bool Result { get; }
        public IGameObject? GameObject { get; }
        
        public ObjectCreateResult(bool result, IGameObject? gameObject)
        {
            Result = result;
            GameObject = gameObject;
        }

        public static ObjectCreateResult Failed = new ObjectCreateResult(false, null);
    }
    
    public interface IObjectManager : IManager
    {
        IMinionManager MinionManager { get; }
        IMonsterManager MonsterManager { get; }
        IPlantManager PlantManager { get; }
        IWardManager WardManager { get; }
        ITrapManager TrapManager { get; }

        IGameObject? GetByNetworkId(int handle);
        IEnumerable<IGameObject> GetGameObjects();
        IEnumerable<IGameObject> GetGameObjects(float range);
        IEnumerable<IGameObject> GetGameObjects(Vector3 position, float range);
    }
}
