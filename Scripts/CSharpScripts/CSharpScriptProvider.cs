using Api.Game.Objects;
using Api.Scripts;

namespace Scripts.CSharpScripts;

public class CSharpScriptProvider : IScriptProvider
{
    private IOrbWalkScript? _orbWalkScript;
    private IChampionScript? _championScript;
    private readonly IList<IScript>? _scripts;
    private readonly ILocalPlayer _localPlayer;
    
    public CSharpScriptProvider(IEnumerable<IScript>? scripts, ILocalPlayer localPlayer)
    {
        _localPlayer = localPlayer;
        _scripts = scripts?.ToList();
        _orbWalkScript = _scripts?.OfType<IOrbWalkScript>().FirstOrDefault();
    }
    public IEnumerable<IScript> GetScripts()
    {
        if (_scripts is not null)
        {
            foreach (var script in _scripts)
            {
                yield return script;
            }
        }
    }

    public IEnumerable<IScript> GetScripts(ScriptType scriptType)
    {
        return GetScripts().Where(x => x.ScriptType == scriptType);
    }

    public void LoadScripts()
    {
    }

    public void Load()
    {
        if(_scripts is null) return;
        foreach (var script in _scripts)
        {
            if (script is IChampionScript championScript)
            {
                script.Enabled = championScript.Champion.GetHashCode() == _localPlayer.ObjectNameHash && _championScript is null;
                if (!script.Enabled)
                {
                    continue;
                }
                
                _championScript = championScript;
            }
            else
            {
                script.Enabled = true;
            }
            script.OnLoad();
        }
    }

    public void Update(float deltaTime)
    {
        if(_scripts is null) return;
        foreach (var script in _scripts.Where(x => x.Enabled))
        {
            script.OnUpdate(deltaTime);
        }
    }

    public void Render(float deltaTime)
    {
        if(_scripts is null) return;
        foreach (var script in _scripts.Where(x => x.Enabled))
        {
            script.OnRender(deltaTime);
        }
    }

    public void Unload()
    {
        if(_scripts is null) return;
        foreach (var script in _scripts)
        {
            script.OnUnload();
        }
    }
}