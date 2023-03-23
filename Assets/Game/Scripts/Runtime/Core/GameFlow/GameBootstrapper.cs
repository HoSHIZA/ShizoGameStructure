using Game.Core.Base.EventBus;
using Game.Core.Base.SceneManagement;
using Game.Core.Base.ServiceLocator;
using UnityEngine;

namespace Game.Core.GameFlow
{
    /// <summary>
    /// The entry point into the game, started when you start the boot scene.
    /// </summary>
    public sealed class GameBootstrapper : MonoBehaviour
    {
        private GameStateMachine _stateMachine;
        
        private void Awake()
        {
            GameRunner.AddToGameContainer(gameObject);
        }
        
        private void Start()
        {
            _stateMachine = new GameStateMachine(new StaticServiceLocator(), new BasicSceneLoader(this), new EventBus());
            
            GameRunner.SetGameStateMachine(_stateMachine);
        }
        
        private void Update()
        {
            _stateMachine.Update();
        }
        
        private void FixedUpdate()
        {
            _stateMachine.FixedUpdate();
        }
        
        private void LateUpdate()
        {
            _stateMachine.LateUpdate();
        }
    }
}