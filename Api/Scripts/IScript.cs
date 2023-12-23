using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Scripts
{
    public interface IScript
    {
        string Name { get; }
        ScriptType ScriptType { get; }
        bool Enabled { get; set; }
        void OnLoad();
        void OnUnload();
        void OnUpdate(float deltaTime);
        void OnRender(float deltaTime);
    }
}
