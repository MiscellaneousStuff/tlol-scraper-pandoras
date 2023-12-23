using System.Numerics;
using Api.Game.Objects;
using Newtonsoft.Json;

namespace Api.Internal.Game.Objects;

internal class LocalPlayer : Hero, ILocalPlayer
{
    
    public override string ToString()
    {
        return JsonConvert.SerializeObject(this, Formatting.Indented);
    }
}