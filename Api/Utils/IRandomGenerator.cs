using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Utils
{
    public interface IRandomGenerator
    {
        float NextFloat();
        float NextFloat(float min, float max);
        float NexInt(int min, int max);
        string GetRandomString(int minLen, int maxLen);
    }
}
