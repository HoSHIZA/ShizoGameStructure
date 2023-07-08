using System;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Game.Core.Base.SceneManagement
{
    /// <summary>
    /// Basic ISceneLoader implementation that loads scenes asynchronously using Task
    /// </summary>
    public sealed class TaskSceneLoader : ISceneLoader
    {
        /// <inheritdoc/>
        public async void LoadScene(string name, bool validateSceneName = true, Action onSceneLoaded = null,
            Action<float> onLoadProgress = null)
        {
            await LoadSceneTask(name, validateSceneName, onSceneLoaded, onLoadProgress);
        }
        
        private async Task LoadSceneTask(string name, bool validateSceneName = true, Action onSceneLoaded = null,
            Action<float> onLoadProgress = null)
        {
            if (validateSceneName && SceneManager.GetActiveScene().name == name) return;

            var loadTask = SceneManager.LoadSceneAsync(name);
            
            loadTask.allowSceneActivation = false;
            loadTask.allowSceneActivation = true;
            
            while (!loadTask.isDone)
            {
                onLoadProgress?.Invoke(loadTask.progress);
                
                await Task.Yield();
            }
            
            onSceneLoaded?.Invoke();
        }
    }
}