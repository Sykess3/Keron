using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure
{
    public class InitialSceneLoader
    {
        private const string Initial = "Initial";
        private readonly ICoroutineRunner _coroutineRunner;

        public InitialSceneLoader(ICoroutineRunner coroutineRunner) => 
            _coroutineRunner = coroutineRunner;
        

        public void Load(Action onLoaded = null) =>
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
    }
}