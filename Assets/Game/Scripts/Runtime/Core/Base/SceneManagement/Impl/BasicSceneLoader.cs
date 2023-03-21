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
            _coroutineRunner.StartCoroutine(LoadSceneTask(name, validateSceneName, onSceneLoaded, onLoadProgress));
        }
        
        private IEnumerator LoadSceneTask(string name, bool validateSceneName = true, Action onSceneLoaded = null,
            Action<float> onLoadProgress = null)
        {
            if (validateSceneName && SceneManager.GetActiveScene().name == name) yield break;
            
            var loadTask = SceneManager.LoadSceneAsync(name);
            
            loadTask.allowSceneActivation = false;
            
            while (!loadTask.isDone)
            {
                onLoadProgress?.Invoke(loadTask.progress);

                yield return null;
            }
            
            loadTask.allowSceneActivation = true;
            
            onSceneLoaded?.Invoke();
        }
    }
}