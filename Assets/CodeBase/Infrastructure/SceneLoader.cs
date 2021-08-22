using System;
using System.Collections;
using System.Collections.Generic;
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

        public SceneLoader(ICoroutineRunner coroutineRunner) => 
            _coroutineRunner = coroutineRunner;
        

        public void Load(string name, LoadSceneMode sceneMode, Action onLoaded = null) =>
            _coroutineRunner.StartCoroutine(LoadScene(name, sceneMode, onLoaded));

        private IEnumerator LoadScene(string nextScene, LoadSceneMode sceneMode, Action onLoaded = null)
        {
            AsyncOperationHandle<SceneInstance> waitNextScene =
                (sceneMode == LoadSceneMode.Additive)
                ? LoadAdditive(nextScene) 
                : LoadSingle(nextScene);

            CacheHandle(waitNextScene);

            while (!waitNextScene.IsDone)
                yield return null;
            onLoaded?.Invoke();
        }

        private static AsyncOperationHandle<SceneInstance> LoadAdditive(string nextScene)
        {
            return Addressables.LoadSceneAsync(nextScene, LoadSceneMode.Additive);
        }

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