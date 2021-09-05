using System.Threading.Tasks;
using CodeBase.Infrastructure.Factory;
using CodeBase.Logic;
using CodeBase.Services.PersistentProgress;
using CodeBase.UI.Services.Factory;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure.GameStates
{
    public class LoadLevelState : IPayloadedGameState<string>
    {
        private const string SceneContext = "SceneContext";
        private readonly ISceneLoader _sceneLoader;
        private readonly LoadingCurtain _loadingCurtain;

        private readonly IGameFactory _gameFactory;
        private readonly IUIFactory _uiFactory;
        private readonly IProgressWatchersContainer _progressWatchersContainer;

        public LoadLevelState(LoadingCurtain loadingCurtain,
            ISceneLoader sceneLoader,
            IUIFactory uiFactory,
            IProgressWatchersContainer progressWatchersContainer,
            IGameFactory gameFactory)
        {
            _sceneLoader = sceneLoader;
            _uiFactory = uiFactory;
            _progressWatchersContainer = progressWatchersContainer;
            _gameFactory = gameFactory;
            _loadingCurtain = loadingCurtain;
        }


        public async void Enter(string payLoaded)
        {
            _progressWatchersContainer.CleanUp();
            _gameFactory.CleanUp();
            _gameFactory.WarmUp();
            _loadingCurtain.Show();
            await _sceneLoader.Load(name: payLoaded, LoadSceneMode.Single, OnLoaded);
        }

        public void Exit() => 
            _loadingCurtain.Hide();

        private async void OnLoaded()
        {
            await InitUIRoot();
            InstantiateSceneContext();
        }

        private static void InstantiateSceneContext()
        {
            GameObject sceneContext = Resources.Load<GameObject>(SceneContext);
            Object.Instantiate(sceneContext);
        }


        private async Task InitUIRoot() =>
            await _uiFactory.CreateUIRoot();

    }
}