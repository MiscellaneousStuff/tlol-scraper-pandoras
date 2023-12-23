using Api.Game.Objects;
using Newtonsoft.Json;

namespace Api.Internal.Game.Objects;

internal class Trap : GameObject, ITrap
{
    public override string ToString()
    {
        return JsonConvert.SerializeObject(this, Formatting.Indented);
    }
}