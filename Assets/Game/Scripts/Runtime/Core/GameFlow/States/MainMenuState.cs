using Game.Core.Base.EventBus;
using Game.Core.Base.SceneManagement;
using Game.Core.Base.ServiceLocator;
using Game.Core.Base.StateMachine;

namespace Game.Core.GameFlow.States
{
    public sealed class MainMenuState : State<GameStateMachine>
    {
        private readonly IServiceLocator _services;
        private readonly ISceneLoader _sceneLoader;
        private readonly IEventBus _eventBus;
        
        public MainMenuState(IServiceLocator services, ISceneLoader sceneLoader, IEventBus eventBus)
        {
            _services = services;
            _sceneLoader = sceneLoader;
            _eventBus = eventBus;
        }
        
        internal override void Enter()
        {
            _sceneLoader.LoadScene("MainMenuScene", onSceneLoaded: OnSceneLoaded);
        }
        
        internal override void Exit()
        {
            
        }
        
        private void OnSceneLoaded()
        {
            
        }
    }
}