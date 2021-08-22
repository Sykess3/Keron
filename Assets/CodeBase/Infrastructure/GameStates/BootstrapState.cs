using System.Collections;
using System.Threading.Tasks;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services.Ads;
using CodeBase.Infrastructure.Services.Randomizer;
using CodeBase.Infrastructure.Services.SaveLoad;
using CodeBase.Services;
using CodeBase.Services.Input;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.StaticData;
using CodeBase.UI.Services.Factory;
using CodeBase.UI.Services.Windows;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure.GameStates
{
    public class BootstrapState : IGameState
    {
        private const string Initial = "Initial";
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly AllServices _services;

        public BootstrapState(GameStateMachine stateMachine, SceneLoader sceneLoader, AllServices services)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _services = services;

            RegisterServices();
        }

        public void Enter()
        {
            LoadInitialScene();
        }

        public void Exit()
        {
        }

        private void EnterLoadProgress() =>
            _stateMachine.Enter<LoadProgressState>();

        private async Task RegisterServices()
        {
            IGameStateMachine gameStateMachine = RegisterGameStateMachine();
            IAssets assets = RegisterAssets();
            IStaticDataService staticData = await RegisterStaticData(assets);
            IRandomizer randomizer = RegisterRandomizer();
            IPersistentProgressService persistentProgress = RegisterPersistentProgress();
            IAdsService adsService = RegisterAdsService();
            IUIFactory uiFactory = RegisterUIFactory(
                assets,
                staticData,
                persistentProgress,
                adsService
            );
            IWindowsService windowsService = RegisterWindowsService(uiFactory);
            ISaveLoadService saveLoadService = RegisterSaveLoad(persistentProgress);
            IGameFactory gameFactory = RegisterGameFactory(
                assets,
                staticData,
                randomizer,
                persistentProgress,
                windowsService,
                gameStateMachine,
                saveLoadService);
            saveLoadService.GameFactory = gameFactory;

            _services.RegisterSingle<IInputService>(new SimpleInputService());
        }

        private ISaveLoadService RegisterSaveLoad(IPersistentProgressService persistentProgress)
        {
            _services.RegisterSingle<ISaveLoadService>(new SaveLoadService(persistentProgress));
            return _services.Single<ISaveLoadService>();
        }

        private IGameStateMachine RegisterGameStateMachine()
        {
            _services.RegisterSingle<IGameStateMachine>(_stateMachine);
            return _services.Single<IGameStateMachine>();
        }

        private IAdsService RegisterAdsService()
        {
            AdsService adsService = new AdsService();
            adsService.Initialize();
            _services.RegisterSingle<IAdsService>(adsService);
            return _services.Single<IAdsService>();
        }

        private IWindowsService RegisterWindowsService(IUIFactory uiFactory)
        {
            _services.RegisterSingle<IWindowsService>(new WindowsService(uiFactory));
            return _services.Single<IWindowsService>();
        }

        private IUIFactory RegisterUIFactory(
            IAssets assets,
            IStaticDataService staticData,
            IPersistentProgressService progressService,
            IAdsService adService)
        {
            _services.RegisterSingle<IUIFactory>(new UIFactory(assets, staticData, progressService, adService));
            return _services.Single<IUIFactory>();
        }

        private IPersistentProgressService RegisterPersistentProgress()
        {
            _services.RegisterSingle<IPersistentProgressService>(new PersistentProgressService());
            return _services.Single<IPersistentProgressService>();
        }

        private IRandomizer RegisterRandomizer()
        {
            _services.RegisterSingle<IRandomizer>(new UnityRandom());
            return _services.Single<IRandomizer>();
        }

        private IGameFactory RegisterGameFactory(IAssets assets,
            IStaticDataService staticData,
            IRandomizer randomizer,
            IPersistentProgressService persistentProgress,
            IWindowsService windowsService,
            IGameStateMachine gameStateMachine,
            ISaveLoadService saveLoadService)
        {
            _services.RegisterSingle<IGameFactory>(new GameFactory(
                assets,
                staticData,
                randomizer,
                persistentProgress,
                windowsService,
                gameStateMachine,
                saveLoadService));
            return _services.Single<IGameFactory>();
        }

        private IAssets RegisterAssets()
        {
            _services.RegisterSingle<IAssets>(new AssetsProvider());

            IAssets assets = _services.Single<IAssets>();
            assets.Initialize();
            return assets;
        }

        private async Task<IStaticDataService> RegisterStaticData(IAssets assets)
        {
            StaticDataService staticDataService = new StaticDataService(assets);
            _services.RegisterSingle<IStaticDataService>(staticDataService);

            await staticDataService.Load();
            return staticDataService;
        }

        private IEnumerator LoadInitialScene()
        {
            if (SceneManager.GetActiveScene().name == Initial)
            {
                EnterLoadProgress();
                yield break;
            }

            AsyncOperation waitNextScene = SceneManager.LoadSceneAsync(Initial);

            while (!waitNextScene.isDone)
                yield return null;
            EnterLoadProgress();
        }
    }
}