using Game.Core.Base.SceneManagement;
using Game.Core.Base.StateMachine;

namespace Game.Core.GameFlow.States
{
    public sealed class MainMenuState : State<GameStateMachine>
    {
        private readonly ISceneLoader _sceneLoader;
        
        public MainMenuState(ISceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
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