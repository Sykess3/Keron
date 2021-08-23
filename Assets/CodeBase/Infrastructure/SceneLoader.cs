using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure
{
    public class SceneLoader
    {
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly List<AsyncOperationHandle<SceneInstance>> _previousScenesHandles = new List<AsyncOperationHandle<SceneInstance>>();
        private const string Initial = "Initial";

        public SceneLoader(ICoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
        }
        
        public async Task Load(string name, LoadSceneMode sceneMode, Action onLoaded = null) =>
            await LoadScene(name, sceneMode, onLoaded);
        
        public void LoadInitial(Action onLoaded = null) =>
            _coroutineRunner.StartCoroutine(LoadScene(Initial, onLoaded));

        private IEnumerator LoadScene(string nextScene, Action onLoaded = null)
        {
            if (SceneManager.GetActiveScene().name == nextScene)
            {
                onLoaded?.Invoke();
                yield break;
            }
            AsyncOperation waitNextScene = SceneManager.LoadSceneAsync(nextScene);

            while (!waitNextScene.isDone)
                yield return null;
            onLoaded?.Invoke();
        }

        private async Task LoadScene(string nextScene, LoadSceneMode sceneMode, Action onLoaded = null)
        {
            AsyncOperationHandle<SceneInstance> waitNextScene =
                (sceneMode == LoadSceneMode.Additive)
                ? LoadAdditive(nextScene) 
                : LoadSingle(nextScene);

            CacheHandle(waitNextScene);

            await waitNextScene.Task;
            onLoaded?.Invoke();
        }

        private static AsyncOperationHandle<SceneInstance> LoadAdditive(string nextScene) => 
            Addressables.LoadSceneAsync(nextScene, LoadSceneMode.Additive);

        private AsyncOperationHandle<SceneInstance> LoadSingle(string nextScene)
        {
            AsyncOperationHandle<SceneInstance> sceneHandle = Addressables.LoadSceneAsync(nextScene);
            CleanUp();
            return sceneHandle;
        }

        private void CleanUp()
        {
            foreach (AsyncOperationHandle<SceneInstance> handle in _previousScenesHandles) 
                Addressables.Release(handle);
        }

        private void CacheHandle(AsyncOperationHandle<SceneInstance> currentScene) => 
            _previousScenesHandles.Add(currentScene);
    }
}