using Api.Game.Objects;
using Api.Scripts;

namespace Scripts.Utils;

public class CurrentTargetSelector : ITargetSelector
{
    public ITargetSelector? TargetSelector { get; set; }

    public string Name => TargetSelector.Name;
    public ScriptType ScriptType => TargetSelector.ScriptType;
    public bool Enabled
    {
        get => TargetSelector.Enabled;
        set => TargetSelector.Enabled = value;
    }

    public void OnLoad()
    {
        TargetSelector.OnLoad();
    }

    public void OnUnload()
    {
        TargetSelector.OnLoad();
    }

    public void OnUpdate(float deltaTime)
    {
        TargetSelector.OnUpdate(deltaTime);
    }

    public void OnRender(float deltaTime)
    {
        TargetSelector.OnRender(deltaTime);
    }

    public TargetSelectorMode TargetSelectorMode
    {
        get => TargetSelector.TargetSelectorMode;
        set => TargetSelector.TargetSelectorMode = value;
    }

    public float HealthWeight
    {
        get => TargetSelector.HealthWeight;
        set => TargetSelector.HealthWeight = value;
    }

    public float AbilityPowerWeight
    {
        get => TargetSelector.AbilityPowerWeight;
        set => TargetSelector.AbilityPowerWeight = value;
    }

    public float DamageWeight
    {
        get => TargetSelector.DamageWeight;
        set => TargetSelector.DamageWeight = value;
    }

    public IHero? GetTarget() => TargetSelector.GetTarget();
    public IHero? GetTarget(float range) => TargetSelector.GetTarget(range);
}