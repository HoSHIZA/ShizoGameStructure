using System;
using Game.Core.Base.ServiceLocator;

namespace Game.Core.Base.SceneManagement
{
    /// <summary>
    /// Defines a contract for loading scenes.
    /// </summary>
    public interface ISceneLoader : IService
    {
        /// <summary>
        /// Loads a scene by name.
        /// </summary>
        /// <param name="name">The name of the scene to load.</param>
        /// <param name="validateSceneName">If set to <c>true</c>, it validates the scene name before loading.</param>
        /// <param name="onSceneLoaded">The callback to be invoked when the scene has been successfully loaded.</param>
        /// <param name="onLoadProgress">The callback to be invoked when the loading progress changes.</param>
        void LoadScene(string name, bool validateSceneName = true, Action onSceneLoaded = null, 
            Action<float> onLoadProgress = null);
    }
}