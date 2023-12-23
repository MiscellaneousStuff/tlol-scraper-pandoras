using Api.Game.Objects;
using Api.Game.ObjectTypes;
using Newtonsoft.Json;

namespace Api.Internal.Game.Objects;

internal class Minion : AiBaseUnit, IMinion
{
    public MinionType MinionType { get; set; }
    
    public override string ToString()
    {
        return JsonConvert.SerializeObject(this, Formatting.Indented);
    }
}