using Api.Game.Objects;

namespace Api.Game.Calculations;

public interface IDamagePrediction
{ 
    float PredictHealth(IAttackableUnit target, float time);
}