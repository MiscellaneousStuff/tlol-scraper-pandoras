using Api.Game.Objects;
using Api.Game.ObjectTypes;
using Newtonsoft.Json;

namespace Api.Internal.Game.Objects;

internal class Plant : AttackableUnit, IPlant
{
    public PlantType PlantType { get; set; }
    
    public override string ToString()
    {
        return JsonConvert.SerializeObject(this, Formatting.Indented);
    }
}