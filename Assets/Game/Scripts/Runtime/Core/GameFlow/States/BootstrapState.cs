using Game.Core.Base.EventBus;
using Game.Core.Base.SceneManagement;
using Game.Core.Base.ServiceLocator;
using Game.Core.Base.StateMachine;

namespace Game.Core.GameFlow.States
{
    public sealed class BootstrapState : State<GameStateMachine>
    {
        private readonly IServiceLocator _services;
        private readonly ISceneLoader _sceneLoader;
        private readonly IEventBus _eventBus;
        
        private bool _initialized;
        
        public BootstrapState(IServiceLocator services, ISceneLoader sceneLoader, IEventBus eventBus)
        {
            _services = services;
            _sceneLoader = sceneLoader;
            _eventBus = eventBus;

            AllServices.Setup(services);
            
            RegisterServices();
        }
        
        internal override void Enter()
        {
            StateMachine.ChangeState<MainMenuState>();
        }
        
        internal override void Exit()
        {
            
        }
        
        private void RegisterServices()
        {
            Register_SceneLoader();
            Register_EventBus();
        }
        
        private void Register_SceneLoader()
        {
            _services.Register<ISceneLoader>(_sceneLoader);
        }
        
        private void Register_EventBus()
        {
            _services.Register<IEventBus>(_eventBus);
        }
    }
}