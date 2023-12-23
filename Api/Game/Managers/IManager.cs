using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Game.Managers
{
    public interface IManager : IDisposable
    {
        void Update(float deltaTime);
    }
}
