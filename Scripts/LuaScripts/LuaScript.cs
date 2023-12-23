using Api.Scripts;
using NLua;

namespace Scripts.LuaScripts
{
    internal class LuaScript : IScript
    {
        public string Name { get; }
        public ScriptType ScriptType { get; }
        public bool Enabled { get; set; }

        private readonly LuaFunction? _onLoad;
        private readonly LuaFunction? _onUnload;
        private readonly LuaFunction? _onUpdate;
        private readonly LuaFunction? _onRender;
        
        public LuaScript(Lua lua, ScriptType scriptType, string file)
        {
            Name = file;
            ScriptType = scriptType;
            Enabled = true;

            try
            {
                lua.DoFile(file);
                _onLoad = lua.GetFunction("OnLoad");
                _onUnload = lua.GetFunction("OnUnload");
                _onUpdate = lua.GetFunction("OnUpdate");
                _onRender = lua.GetFunction("OnRender");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        
        public void OnLoad()
        {
            _onLoad?.Call();
        }

        public void OnUnload()
        {
            _onUnload?.Call();
        }

        public void OnUpdate(float deltaTime)
        {
            _onUpdate?.Call(deltaTime);
        }

        public void OnRender(float deltaTime)
        {
            _onRender?.Call(deltaTime);
        }
    }
}
