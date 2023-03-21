using Game.Core.Base.SceneManagement;
using UnityEngine;

namespace Game.Core.GameFlow
{
    public sealed class GameBootstrapper : MonoBehaviour
    {
        private void Awake()
        {
            GameRunner.AddToGameContainer(gameObject);
            
            var stateMachine = new GameStateMachine(new BasicSceneLoader(this));
            
            GameRunner.SetGameStateMachine(stateMachine);
        }
    }
}