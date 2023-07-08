using UnityEngine;

namespace Game.Core.GameFlow
{
    [CreateAssetMenu(fileName = "GameRuntimeConfig", menuName = "Game/Internal/Game Runtime Configuration")]
    internal class GameRuntimeConfig : ScriptableObject
    {
        /// <summary>
        /// The path of the GameRuntimeConfig asset.
        /// </summary>
        public const string GAME_RUNTIME_CONFIG_PATH = "Internal/GameRuntimeConfig";
        
        [Header("Boot")]
        
        [Tooltip("If true, you will not be allowed to add objects to the game container outside the boot scene. It works only if the boot scene is defined and correct.")]
        [SerializeField, Space] private bool _disallowAddingToGameContainerOutsideBootScene = true;
        
        [Tooltip("The name of the boot scene.")]
        [SerializeField] private string _bootScene = "";

        [Header("Debugging")]
        
        [Tooltip("Whether logging is enabled for important components responsible for loading and controlling game flow.")]
        [SerializeField] private bool _logging = true;
        
        [Tooltip("Whether logging is enabled for the GameStateMachine.")]
        [SerializeField] private bool _gameStateMachineLogging = true;

        /// <summary>
        /// Indicates whether objects can be added to the game container outside the boot scene.
        /// </summary>
        public bool DisallowAddingToGameContainerOutsideBootScene => _disallowAddingToGameContainerOutsideBootScene;
        
        /// <summary>
        /// Gets the name of the boot scene.
        /// </summary>
        public string BootScene => _bootScene;
        
        /// <summary>
        /// Indicates whether logging is enabled for the game.
        /// </summary>
        public bool Logging => _logging;
        
        /// <summary>
        /// Indicates whether logging is enabled for the GameStateMachine.
        /// </summary>
        public bool GameStateMachineLogging => _gameStateMachineLogging;

        /// <summary>
        /// Loads a <see cref="GameRuntimeConfig"/> from resources.
        /// </summary>
        /// <param name="customPath">Optional custom path to the resource asset.</param>
        /// <returns>The loaded <see cref="GameRuntimeConfig"/> object.</returns>
        public static GameRuntimeConfig LoadFromResources(string customPath = null)
        {
            if (!string.IsNullOrEmpty(customPath))
            {
                var configFromCustomPath = Resources.Load<GameRuntimeConfig>(customPath);

                if (configFromCustomPath)
                {
                    return configFromCustomPath;
                }
            }
            
            var configFromDefaultPath = Resources.Load<GameRuntimeConfig>(GAME_RUNTIME_CONFIG_PATH);
            
            if (configFromDefaultPath == null) configFromDefaultPath = CreateInstance<GameRuntimeConfig>();
            
            return configFromDefaultPath;
        }
    }
}