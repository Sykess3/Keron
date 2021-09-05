using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.GameStates;
using CodeBase.Infrastructure.Services.Ads;
using CodeBase.Infrastructure.Services.IAP;
using CodeBase.Infrastructure.Services.Randomizer;
using CodeBase.Infrastructure.Services.SaveLoad;
using CodeBase.Logic;
using CodeBase.Services.Input;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.StaticData;
using CodeBase.UI.Services.Factory;
using CodeBase.UI.Services.Windows;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure
{
    public class BootstrapInstaller : MonoInstaller, IInitializable, ICoroutineRunner
    {
        [SerializeField] private LoadingCurtain _curtainPrefab;
        [SerializeField] private GameBootstrapper _gameBootstrapperPrefab;

        public override void InstallBindings()
        {
            Bind_InstallerInterfaces();
            Bind_LoadingCurtain();
            Bind_SceneLoader();
            Bind_PersistentProgress();
            Bind_ProgressWatchers();

            Bind_AssetsProvider();
            Bind_StaticData();
            Bind_IAPProvider();
            Bind_IAPService();
            Bind_Randomizer();
            Bind_Ads();
            Bind_WindowsService();
            Bind_SaveLoad();
            Bind_GameFactory();
            Bind_UIFactory();
            Bind_InputService();
            Bind_GameStateMachine();
        }

        public void Initialize()
        {
            GameBootstrapper bootstrapper = FindObjectOfType<GameBootstrapper>();

            if (bootstrapper == null) 
                Container.InstantiatePrefab(_gameBootstrapperPrefab);
        }

        private void Bind_ProgressWatchers()
        {
            Container.
                BindInterfacesTo<ProgressWatchers>()
                .AsSingle();
        }

        private void Bind_InputService()
        {
            Container
                .Bind<IInputService>()
                .To<SimpleInputService>()
                .AsSingle();
        }

        private void Bind_UIFactory()
        {
            Container
                .Bind<IUIFactory>()
                .To<UIFactory>()
                .AsSingle();
        }

        private void Bind_SaveLoad()
        {
            Container
                .Bind<ISaveLoadService>()
                .To<SaveLoadService>()
                .AsSingle();
        }

        private void Bind_GameFactory()
        {
            Container
                .Bind<IGameFactory>()
                .To<GameFactory>()
                .AsSingle();
        }

        private void Bind_WindowsService()
        {
            Container
                .Bind<IWindowsService>()
                .To<WindowsService>()
                .AsSingle();
        }

        private void Bind_Ads()
        {
            Container
                .Bind<IAdsService>()
                .To<AdsService>()
                .AsSingle()
                .OnInstantiated<AdsService>((ctx, ads) => ads.Initialize());
        }

        private void Bind_Randomizer()
        {
            Container
                .Bind<IRandomizer>()
                .To<UnityRandom>()
                .AsSingle();
        }

        private void Bind_IAPProvider()
        {
            Container
                .Bind<IAPProvider>()
                .AsSingle();
        }

        private void Bind_IAPService()
        {
            Container
                .Bind<IIAPService>()
                .To<IAPService>()
                .AsSingle();
        }

        private void Bind_PersistentProgress()
        {
            Container
                .Bind<IPersistentProgressService>()
                .To<PersistentProgressService>()
                .AsSingle();
        }
        private void Bind_StaticData()
        {
            Container
                .Bind<IStaticDataService>()
                .To<StaticDataService>()
                .AsSingle();
        }

        private void Bind_AssetsProvider()
        {
            Container
                .Bind<IAssets>()
                .To<AssetsProvider>()
                .AsSingle()
                .OnInstantiated<AssetsProvider>((ctx, assets) => assets.Initialize());
        }

        private void Bind_SceneLoader()
        {
            Container
                .Bind<ISceneLoader>()
                .To<SceneLoader>()
                .AsSingle();
        }


        private void Bind_GameStateMachine()
        {
            Container
                .Bind<IGameStateMachine>()
                .To<GameStateMachine>()
                .AsSingle();
        }

        private void Bind_LoadingCurtain()
        {
            Container
                .Bind<LoadingCurtain>()
                .FromComponentInNewPrefab(_curtainPrefab)
                .AsSingle();
        }

        private void Bind_InstallerInterfaces()
        {
            Container
                .BindInterfacesTo<BootstrapInstaller>()
                .FromInstance(this)
                .AsSingle();
        }
    }
}