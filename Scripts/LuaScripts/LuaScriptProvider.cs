using System.Numerics;
using Api;
using Api.Game.Managers;
using Api.Inputs;
using Api.Scripts;
using Api.Utils;

using Microsoft.Extensions.Logging;
using NLua;
using Scripts.Extensions;

namespace Scripts.LuaScripts
{
    internal class LuaScriptProvider : IScriptProvider, IDisposable
    {
        private readonly ILogger<LuaScriptProvider> _logger;
        private readonly List<LuaScript> _scripts;
        private readonly LuaBinder _luaBinder;
        private Lua? _lua;
        private float _nextGc = 0;
        private IEnumerable<LuaScript> EnabledScripts => _scripts.Where(x => x.Enabled);
        
        public LuaScriptProvider(ILogger<LuaScriptProvider> logger, LuaBinder luaBinder)
        {
            _logger = logger;
            _luaBinder = luaBinder;
            _scripts = new List<LuaScript>();
        }
        
        public IEnumerable<IScript> GetScripts()
        {
            return _scripts;
        }

        public IEnumerable<IScript> GetScripts(ScriptType scriptType)
        {
            return _scripts.Where(x => x.ScriptType == scriptType);
        }

        public void LoadScripts()
        {
            _disposed = false;
            _lua = _luaBinder.Create();

            _lua?.DoString("collectgarbage(\"setpause\", 100)");
            _lua?.DoString("collectgarbage(\"setstepmul\", 5000)");

            _logger.LogInformation("Load Scripts");
            foreach (var scriptType in Enum.GetValues<ScriptType>())
            {
                LoadScripts(scriptType);
            }
        }

        private void LoadScripts(ScriptType scriptType)
        {
            if(_lua is null) return;
            
            var utilityLuaScripts = FileService.GetFilesExtension("lua", "Resources", "LuaScripts", scriptType.ToString());
            foreach (var utilityLuaScript in utilityLuaScripts)
            {
                if (string.IsNullOrWhiteSpace(utilityLuaScript))
                {
                    continue;
                }
                
                _logger.LogInformation(utilityLuaScript);
                _scripts.Add(new LuaScript(_lua, scriptType, utilityLuaScript));
            }
        }

        public void Load()
        {
            foreach (var luaScript in EnabledScripts)
            {
                luaScript.OnLoad();
            }
            _lua?.DoString("collectgarbage('collect')");
        }

        public void Update(float deltaTime)
        {
            foreach (var luaScript in EnabledScripts)
            {
                luaScript.OnUpdate(deltaTime);
            }
            GcLua(deltaTime);
        }

        public void Render(float deltaTime)
        {
            foreach (var luaScript in EnabledScripts)
            {
                luaScript.OnRender(deltaTime);
            }
            GcLua(deltaTime);
        }

        private void GcLua(float deltaTime)
        {
            _nextGc -= deltaTime;
            if (_nextGc <= 0)
            {
                _lua?.DoString("collectgarbage('collect')");
                _nextGc = 1f;
            }
        }

        public void Unload()
        {
            foreach (var luaScript in EnabledScripts)
            {
                luaScript.OnUnload();
            }

            Dispose(true);
        }

        private bool _disposed;
        private void Dispose(bool disposing)
        {
            if(_disposed || _lua is null) return;
            _lua?.DoString("collectgarbage('collect')");
            _lua?.Dispose();
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~LuaScriptProvider()
        {
            Dispose(false);
        }
    }
}