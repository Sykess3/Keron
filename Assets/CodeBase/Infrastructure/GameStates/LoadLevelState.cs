using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.CameraLogic;
using CodeBase.Data;
using CodeBase.Hero;
using CodeBase.Infrastructure.Factory;
using CodeBase.Logic;
using CodeBase.Logic.Loot;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.StaticData;
using CodeBase.StaticData;
using CodeBase.UI.Elements;
using CodeBase.UI.Services.Factory;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure.GameStates
{
    public class LoadLevelState : IPayloadedGameState<string>
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly LoadingCurtain _loadingCurtain;

        private readonly IGameFactory _gameFactory;
        private readonly IPersistentProgressService _progress;
        private readonly IStaticDataService _staticData;
        private readonly IUIFactory _uiFactory;

        public LoadLevelState(
            GameStateMachine gameStateMachine, 
            IGameFactory gameFactory,
            IPersistentProgressService progress,
            IStaticDataService staticData,
            LoadingCurtain loadingCurtain,
            SceneLoader sceneLoader,
            IUIFactory uiFactory)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _uiFactory = uiFactory;
            _loadingCurtain = loadingCurtain;
            _gameFactory = gameFactory;
            _progress = progress;
            _staticData = staticData;
        }


        public async void Enter(string payLoaded)
        {
            await _sceneLoader.Load(name: payLoaded, LoadSceneMode.Single, OnLoaded);
            _gameFactory.CleanUp();
            _gameFactory.WarmUp();
            _loadingCurtain.Show();
        }

        public void Exit() => 
            _loadingCurtain.Hide();

        private async void OnLoaded()
        {
            await InitUIRoot();
            await InitGameWorld();

            InformProgressReaders();

            _gameStateMachine.Enter<GameLoopState>();
        }

        private void InformProgressReaders()
        {
            foreach (var progressReader in _gameFactory.ProgressReaders)
                progressReader.LoadProgress(@from: _progress.Progress);
        }

        private async Task InitUIRoot() => 
            await _uiFactory.CreateUIRoot();

        private async Task InitGameWorld()
        {
            
            var levelData = LevelStaticData();

            await InitSpawners(levelData);
            await InitSavedLootInScene();

            GameObject hero = await InitHero(levelData);
            
            await InitHud(hero);
            
            InitTriggers();
            CameraFollow(hero);
        }

        private void InitTriggers()
        {
            _gameFactory.CreateSaveTrigger(CurrentSceneName());
            _gameFactory.CreateTransferLevelTrigger(CurrentSceneName());
        }

    private LevelStaticData LevelStaticData()
        {
            string sceneKey = CurrentSceneName();
            LevelStaticData levelData = _staticData.ForLevel(sceneKey);
            return levelData;
        }

    private async Task InitSpawners(LevelStaticData levelData)
        {

            foreach (EnemySpawnerData spawner in levelData.Spawners)
            {
                await _gameFactory.CreateSpawner(
                    spawner.Position,
                    spawner.UniqueId,
                    spawner.MonsterTypeId
                    );
            }
        }

    private async Task InitSavedLootInScene()
        {
            foreach (KeyValuePair<string, LootPieceData> item in _progress.Progress.WorldData.NonPickedUpLoot)
            {
                LootPiece lootPiece = await _gameFactory.CreateLoot(Vector3.zero);
                
                lootPiece.GetComponent<UniqueId>().Id = item.Key;
                lootPiece.Initialize(item.Value.LootData);
                lootPiece.transform.position = item.Value.Position.AsUnityVector3();
            }
        }

    private async Task<GameObject> InitHero(LevelStaticData levelData) =>
            await _gameFactory.CreateHero(levelData.InitialPoint);

    private async Task InitHud(GameObject hero)
        {
            GameObject hud = await _gameFactory.CreateHud();

            hud.GetComponentInChildren<ActorUI>()
                .Construct(hero.GetComponent<HeroHealth>());
        }

    private void CameraFollow(GameObject hero)
        {
            Camera.main
                .GetComponent<CameraFollow>()
                .Follow(hero);
        }

    private static string CurrentSceneName() => 
        SceneManager.GetActiveScene().name;
    }
}