using Game.Core.Base.SceneManagement;
using UnityEngine;

namespace Game.Core.GameFlow
{
    public sealed class GameBootstrapper : MonoBehaviour
    {
        private GameStateMachine _stateMachine;
        
        private void Awake()
        {
            GameRunner.AddToGameContainer(gameObject);
            
            _stateMachine = new GameStateMachine(new BasicSceneLoader(this));
            
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