using System.Numerics;
using Api.Game.Objects;

namespace Api.Game.Managers;

public interface ITurretManager : IManager
{
    IEnumerable<ITurret> GetAllyTurrets();
    IEnumerable<ITurret> GetAllyTurrets(float range);
    IEnumerable<ITurret> GetAllyTurrets(Vector3 position, float range);
    
    IEnumerable<ITurret> GetEnemyTurrets();
    IEnumerable<ITurret> GetEnemyTurrets(float range);
    IEnumerable<ITurret> GetEnemyTurrets(Vector3 position, float range);
}