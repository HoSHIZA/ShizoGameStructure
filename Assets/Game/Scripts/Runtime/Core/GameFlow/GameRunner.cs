using System;
using Game.Core.Base.StateMachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace Game.Core.GameFlow
{
    /// <summary>
    /// The GameRunner is responsible for managing the game state machine, loading the first scene on runtime initialization,
    /// and handling requests to change the game state.
    /// </summary>
    public static class GameRunner
    {
        private static GameRuntimeConfig _runtimeConfig;
        private static GameStateMachine _stateMachine;
        private static Transform _gameContainer;
        
        /// <summary>
        /// Gets the game runtime config.
        /// </summary>
        internal static GameRuntimeConfig RuntimeConfig => _runtimeConfig 
            ? _runtimeConfig 
            : _runtimeConfig = GameRuntimeConfig.LoadFromResources();
        
        /// <summary>
        /// Gets the current state of the game state machine.
        /// </summary>
        public static Type CurrentState => _stateMachine.GetCurrentStateType();

        /// <summary>
        /// True, if current scene is boot scene.
        /// </summary>
        public static bool CurrentSceneIsBoot => SceneManager.GetActiveScene().name == RuntimeConfig.GetBootScene();
        
        /// <summary>
        /// Loads the boot scene of the game after all assemblies have been loaded.
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void LoadBootScene()
        {
            _runtimeConfig = GameRuntimeConfig.LoadFromResources();
            
            if (!RuntimeConfig.AlwaysStartGameFromBootScene) return;
            if (CurrentSceneIsBoot) return;
            
            var bootScene = RuntimeConfig.GetBootScene();
            
            if (string.IsNullOrEmpty(bootScene))
            {
#if UNITY_EDITOR
                Debug.LogWarning("<color=cyan>[GAME]</color> Boot scene cannot be loaded.\n" +
                                 "Perhaps it does not exist, or it is not added to the build settings");
#endif
                
                return;
            }
            
            SceneManager.LoadScene(bootScene);
        }
        
        /// <summary>
        /// Init everything at the start of the game and the loading scene.
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
            _gameContainer = new GameObject("[GAME]").transform;
            
            Object.DontDestroyOnLoad(_gameContainer);
        }
        
        /// <summary>
        /// Requests a change in the game state to the specified state.
        /// </summary>
        /// <typeparam name="TState">The type of state to transition to.</typeparam>
        public static void RequestState<TState>() where TState : State<GameStateMachine>
        {
            RequestState(typeof(TState));
        }
        
        /// <summary>
        /// Requests a change in the game state to the specified state.
        /// </summary>
        /// <param name="state">The type of state to transition to.</param>
        public static void RequestState(Type state)
        {
            if (CurrentState == state) return;

            _stateMachine.ChangeState(state);
        }
        
        /// <summary>
        /// A method for adding a target to the game container.
        /// If enabled DisallowAddingToGameContainerOutsideBootScene in GameRuntimeConfig,
        /// the game container will only be allowed to update from the boot scene.
        /// </summary>
        /// <param name="target">The object to be added to the game container.</param>
        internal static void AddToGameContainer(GameObject target)
        {
            if (RuntimeConfig.DisallowAddingToGameContainerOutsideBootScene && !CurrentSceneIsBoot)
            {
                if (RuntimeConfig.Logging)
                {
                    Debug.LogWarning("<color=cyan>[GAME]</color> <color=orange>\"AddToGameContainer\"</color> method cannot be called outside the boot scene.");
                }

                return;
            }

            target.transform.SetParent(_gameContainer);
        }
        
        /// <summary>
        /// Sets the game state machine that will be managed by the GameRunner.
        /// </summary>
        /// <param name="gameStateMachine">The game state machine to be set.</param>
        internal static void SetGameStateMachine(GameStateMachine gameStateMachine)
        {
            if (_stateMachine != null) return;

            _stateMachine = gameStateMachine;
        }
    }
}