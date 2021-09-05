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
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace CodeBase.Infrastructure.GameStates
{
    public class InitGameWorldState : IGameState
    {
        private readonly IGameFactory _gameFactory;
        private readonly IStaticDataService _staticData;
        private readonly IPersistentProgressService _persistentProgress;
        private readonly IProgressWatchersContainer _progressWatchersContainer;
        
        private readonly LazyInject<IGameStateMachine> _stateMachine;


        public InitGameWorldState(
            LazyInject<IGameStateMachine> stateMachine,
            IGameFactory gameFactory,
            IStaticDataService staticData,
            IPersistentProgressService persistentProgress,
            IProgressWatchersContainer progressWatchersContainer)
        {
            _stateMachine = stateMachine;
            _gameFactory = gameFactory;
            _staticData = staticData;
            _persistentProgress = persistentProgress;
            _progressWatchersContainer = progressWatchersContainer;
        }

        public async void Enter()
        {
            await InitGameWorld();
            
            _stateMachine.Value.Enter<GameLoopState>();
        }

        public void Exit() => 
            InformProgressReaders();

        private void InformProgressReaders() => 
            _progressWatchersContainer.InformProgressReaders();

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
            NonPickedUpLoot loot = _persistentProgress.Progress.WorldData.NonPickedUpLoot[SceneManager.GetActiveScene().name];
            foreach (KeyValuePair<string, LootPieceData> item in loot)
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

