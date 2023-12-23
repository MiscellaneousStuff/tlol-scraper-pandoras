using Api.Game.Managers;
using Api.Game.Objects;
using Api.Menus;
using Api.Scripts;
using Scripts.Utils;

namespace Scripts
{
    public class ScriptManager : IScriptManager
    {
        private readonly IList<IScriptProvider> _scriptsProviders;
        private IScript _orbwalker;
        private IScript _evade;
        private IMainMenu _mainMenu;
        private readonly IGameManager _gameManager;
        private readonly ITargetSelector _currentTargetSelector;
        private bool _loaded;

        public ScriptManager(
            IEnumerable<IScriptProvider> scriptsProviders,
            IMainMenu mainMenu,
            IGameManager gameManager,
            ITargetSelector currentTargetSelector)
        {
            _mainMenu = mainMenu;
            _gameManager = gameManager;
            _currentTargetSelector = currentTargetSelector;
            _scriptsProviders = scriptsProviders.ToList();
            _gameManager.GameLoaded += Load;
            _loaded = false;
        }

        public IEnumerable<IScript> GetScripts()
        {
            return _scriptsProviders.SelectMany(x => x.GetScripts());
        }

        public IEnumerable<IScript> GetScripts(ScriptType scriptType)
        {
            return _scriptsProviders.SelectMany(x => x.GetScripts(scriptType));
        }

        public void LoadScripts()
        {
            foreach (var scriptsProvider in _scriptsProviders)
            {
                scriptsProvider.LoadScripts();
            }
        }

        public void Load()
        {
            _loaded = true;
            ((_currentTargetSelector as CurrentTargetSelector)!).TargetSelector = _scriptsProviders
                .SelectMany(x => x.GetScripts()).FirstOrDefault(x => x.ScriptType == ScriptType.TargetSelector) as ITargetSelector;
            
            foreach (var scriptsProvider in _scriptsProviders)
            {
                scriptsProvider.Load();
            }

            _mainMenu.LoadSettings();
        }

        public void Update(float deltaTime)
        {
            if(!_loaded) return;
            foreach (var scriptsProvider in _scriptsProviders)
            {
                scriptsProvider.Update(deltaTime);
            }
        }

        public void Render(float deltaTime)
        {
            foreach (var scriptsProvider in _scriptsProviders)
            {
                scriptsProvider.Render(deltaTime);
            }
        }

        public void Unload()
        {
            foreach (var scriptsProvider in _scriptsProviders)
            {
                scriptsProvider.Unload();
            }

            _loaded = false;
        }
    }
}