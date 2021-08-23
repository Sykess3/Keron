using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.Enemy;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.GameStates;
using CodeBase.Infrastructure.Services.Randomizer;
using CodeBase.Infrastructure.Services.SaveLoad;
using CodeBase.Logic.EnemySpawners;
using CodeBase.Logic.Loot;
using CodeBase.Logic.SaveLoad;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.StaticData;
using CodeBase.StaticData;
using CodeBase.UI.Elements;
using CodeBase.UI.Services.Windows;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
    public class GameFactory : IGameFactory
    {
        private readonly IAssets _assets;
        private readonly IRandomizer _randomizer;
        private readonly IStaticDataService _staticData;
        private readonly IPersistentProgressService _persistentProgress;
        private readonly IWindowsService _windowsService;

        private readonly List<ISavedProgressReader> _progressReaders = new List<ISavedProgressReader>();
        private readonly List<ISavedProgress> _progressWritersAndReaders = new List<ISavedProgress>();

        private GameObject _heroGameObject;
        private readonly IGameStateMachine _stateMachine;
        private readonly ISaveLoadService _saveLoadService;

        public IEnumerable<ISavedProgressReader> ProgressReaders => _progressReaders;
        public IEnumerable<ISavedProgress> ProgressWritersAndReaders => _progressWritersAndReaders;

        public GameFactory(IAssets assets,
            IStaticDataService staticData,
            IRandomizer randomizer,
            IPersistentProgressService persistentProgress,
            IWindowsService windowsService,
            IGameStateMachine stateMachine,
            ISaveLoadService saveLoadService)
        {
            _assets = assets;
            _staticData = staticData;
            _randomizer = randomizer;
            _persistentProgress = persistentProgress;
            _windowsService = windowsService;
            _stateMachine = stateMachine;
            _saveLoadService = saveLoadService;
        }

        public async void CreateTransferLevelTrigger(string currentScene)
        {
            LevelStaticData levelData = _staticData.ForLevel(currentScene);

            GameObject transferTriggerPrefab = await _assets.LoadSingle<GameObject>(AssetAddress.TransferLevelTrigger);
            GameObject transferTriggerObject = Object.Instantiate(transferTriggerPrefab, levelData.LevelTransferTrigger.Position, Quaternion.identity);

            transferTriggerObject.GetComponent<BoxCollider>().size = levelData.LevelTransferTrigger.ColliderSize;

            transferTriggerObject.GetComponent<LevelTransferTrigger>().Construct(_stateMachine, levelData.NextSceneKey);
        }

        public async void CreateSaveTrigger(string currentScene)
        {
            LevelStaticData levelData = _staticData.ForLevel(currentScene);

            GameObject saveTriggerPrefab = await _assets.LoadSingle<GameObject>(AssetAddress.SaveTrigger);
            GameObject saveTriggerObject = Object.Instantiate(saveTriggerPrefab, levelData.SaveTrigger.Position, Quaternion.identity);

            saveTriggerObject.GetComponent<BoxCollider>().size = levelData.SaveTrigger.ColliderSize;
            
            saveTriggerObject.GetComponent<SaveTrigger>().Construct(_saveLoadService);
        }

        public async Task<GameObject> CreateHero(Vector3 at)
        {
            _heroGameObject = await InstantiateRegisteredAsync( AssetAddress.Hero, at);
            return _heroGameObject;
        }

        public async Task<GameObject> CreateHud()
        {
            GameObject hud = await InstantiateRegisteredAsync( AssetAddress.Hud);
            InitMoneyCounter(hud);
            InitHudButtons(hud);
            return hud;
        }

        private void InitHudButtons(GameObject hud)
        {
            foreach (OpenWindowButton window in hud.GetComponentsInChildren<OpenWindowButton>())
                window.Construct(_windowsService);
        }


        public async Task<GameObject> CreateMonster(MonsterTypeId monsterTypeId, Transform parent)
        {
            MonsterStaticData monsterData = _staticData.ForMonster(monsterTypeId);

            var prefab = await _assets.LoadSingle<GameObject>(monsterData.PrefabReference);
            
            GameObject monster = Object.Instantiate(prefab, parent.position, Quaternion.identity, parent);

            return InitMonsterComponents(monster, monsterData);
        }

        public async Task<LootPiece> CreateLoot(Vector3 at)
        {
            GameObject prefab = await _assets.LoadSingle<GameObject>(AssetAddress.Loot);
            GameObject loot = InstantiateRegistered(prefab, at);
            return loot.GetComponent<LootPiece>();
        }

        public async Task<SpawnPoint> CreateSpawner(Vector3 at, string uniqueId, MonsterTypeId monsterTypeId)
        {
            GameObject prefab = await _assets.LoadSingle<GameObject>(AssetAddress.SpawnPoint);
            SpawnPoint spawner = InstantiateRegistered(prefab, at)
                .GetComponent<SpawnPoint>();
            
            spawner.Construct(factory: this, monsterTypeId, uniqueId);
            return spawner;
        }

        public async void WarmUp()
        {
            await _assets.LoadSingle<GameObject>(AssetAddress.Loot);
            await _assets.LoadSingle<GameObject>(AssetAddress.SpawnPoint);
            await _assets.LoadSingleForEntireLiceCycle<GameObject>(AssetAddress.SaveTrigger);
            await _assets.LoadSingleForEntireLiceCycle<GameObject>(AssetAddress.TransferLevelTrigger);
        }

        public void CleanUp()
        {
            _progressReaders.Clear();
            _progressWritersAndReaders.Clear();
            _assets.CleanUp();
        }

        private GameObject InstantiateRegistered(GameObject prefab, Vector3 at)
        {
            GameObject gameObject = Object.Instantiate(prefab, at, Quaternion.identity);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        private async Task<GameObject> InstantiateRegisteredAsync(string address, Vector3 at)
        {
            GameObject gameObject = await _assets.Instantiate(address, at);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        private async Task<GameObject> InstantiateRegisteredAsync(string prefabAddress)
        {
            GameObject gameObject = await _assets.Instantiate(prefabAddress);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        private GameObject InstantiateRegistered(GameObject prefab)
        {
            GameObject gameObject = Object.Instantiate(prefab);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        private void Register(ISavedProgressReader progressReader)
        {
            if (progressReader is ISavedProgress progressWriter)
                _progressWritersAndReaders.Add(progressWriter);

            _progressReaders.Add(progressReader);
        }

        private void RegisterProgressWatchers(GameObject gameObject)
        {
            foreach (var progress in gameObject.GetComponentsInChildren<ISavedProgressReader>())
                Register(progress);
        }

        private void InitMoneyCounter(GameObject hud)
        {
            hud.GetComponentInChildren<MoneyCounter>()
                .Construct(_persistentProgress.Progress.Money);
        }

        private GameObject InitMonsterComponents(GameObject monster, MonsterStaticData monsterData)
        {
            monster.GetComponent<Attack>().Construct(
                _heroGameObject.transform,
                monsterData.Attack.Damage,
                monsterData.Attack.AttackCooldown,
                monsterData.Attack.CleavageAttack,
                monsterData.Attack.EffectiveDistance
            );

            IEnemyHealth health = monster.GetComponent<IEnemyHealth>();
            health.Construct(monsterData.HP);
            
            monster.GetComponent<ActorUI>().Construct(health);
            monster.GetComponent<Followable>().Construct(_heroGameObject.transform, monsterData.Speed);
            var lootSpawner = monster.GetComponentInChildren<LootSpawner>();
            lootSpawner.Construct(this, _randomizer);
            lootSpawner.SetLoot(monsterData.Loot.MaxValue, monsterData.Loot.MinValue);

            return monster;
        }
    }
}