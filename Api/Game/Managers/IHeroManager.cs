using System.Numerics;
using Api.Game.Objects;

namespace Api.Game.Managers;

public interface IHeroManager : IManager
{
    IEnumerable<IHero> GetHeroes();
    
    IEnumerable<IHero> GetAllyHeroes();
    IEnumerable<IHero> GetAllyHeroes(float range);
    IEnumerable<IHero> GetAllyHeroes(Vector3 position, float range);
    
    IEnumerable<IHero> GetEnemyHeroes();
    IEnumerable<IHero> GetEnemyHeroes(float range);
    IEnumerable<IHero> GetEnemyHeroes(Vector3 position, float range);

    public IHero? GetHero(int networkId);
}