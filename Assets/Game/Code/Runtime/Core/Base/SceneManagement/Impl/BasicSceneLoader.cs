using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Core.Base.SceneManagement
{
    /// <summary>
    /// Basic ISceneLoader implementation that loads scenes asynchronously using Coroutine
    /// </summary>
    public sealed class BasicSceneLoader : ISceneLoader
    {
        private readonly MonoBehaviour _coroutineRunner;
        
        public BasicSceneLoader(MonoBehaviour obj)
        {
            _coroutineRunner = obj;
        }
        
        /// <inheritdoc/>
        public void LoadScene(string name, bool validateSceneName = true, Action onSceneLoaded = null,
            Action<float> onLoadProgress = null)
        {
            _coroutineRunner.StartCoroutine(LoadSceneCoroutine(name, validateSceneName, onSceneLoaded, onLoadProgress));
        }
        
        private IEnumerator LoadSceneCoroutine(string name, bool validateSceneName = true, Action onSceneLoaded = null,
            Action<float> onLoadProgress = null)
        {
            if (validateSceneName && SceneManager.GetActiveScene().name == name) yield break;
            
            var loadOperation = SceneManager.LoadSceneAsync(name);
            
            loadOperation.allowSceneActivation = false;
            loadOperation.allowSceneActivation = true;
            
            while (!loadOperation.isDone)
            {
                onLoadProgress?.Invoke(loadOperation.progress);

                yield return null;
            }

            onSceneLoaded?.Invoke();
        }
    }
}