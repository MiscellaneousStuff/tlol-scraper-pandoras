namespace Api.Scripts;

public interface IScriptProvider
{
    public IEnumerable<IScript> GetScripts();
    public IEnumerable<IScript> GetScripts(ScriptType scriptType);
    public void LoadScripts();
    public void Load();
    public void Update(float deltaTime);
    public void Render(float deltaTime);
    public void Unload();
}