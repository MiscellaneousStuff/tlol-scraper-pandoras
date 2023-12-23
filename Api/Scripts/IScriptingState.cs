using Api.Inputs;

namespace Api.Scripts;

[Flags]
public enum ActionType
{
    None = 0,
    Combo = 1 << 0,
    Haras = 1 << 1,
    Farm = 1 << 2,
    Clear = 1 << 3,
}

public interface IScriptingState
{
    VirtualKey ComboKey { get; set; }
    
    VirtualKey HarasKey { get; set; }
    
    VirtualKey FarmKey { get; set; }
    
    VirtualKey ClearKey { get; set; }
    ActionType ActionType { get; }
    bool IsCombo { get; }
    bool IsHaras { get; }
    bool IsFarm { get; }
    bool IsClear { get; }
}