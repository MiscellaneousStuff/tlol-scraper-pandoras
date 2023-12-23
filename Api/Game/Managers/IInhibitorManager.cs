using System.Numerics;
using Api.Game.Objects;

namespace Api.Game.Managers;

public interface IInhibitorManager : IManager
{
    IEnumerable<IInhibitor> GetAllInhibitors();
    IEnumerable<IInhibitor> GetAllInhibitors(float range);
    IEnumerable<IInhibitor> GetAllInhibitors(Vector3 position, float range);
    
    IEnumerable<IInhibitor> GetEnemyInhibitors();
    IEnumerable<IInhibitor> GetEnemyInhibitors(float range);
    IEnumerable<IInhibitor> GetEnemyInhibitors(Vector3 position, float range);
}