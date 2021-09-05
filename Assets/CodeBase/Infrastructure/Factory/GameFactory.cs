using System.Threading.Tasks;
using CodeBase.Data;
using CodeBase.Enemy;
using CodeBase.Hero;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Logic.EnemySpawners;
using CodeBase.Logic.Loot;
using CodeBase.Logic.SaveLoad;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.StaticData;
using CodeBase.StaticData;
using CodeBase.UI.Elements;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace CodeBase.Infrastructure.Factory
{
    public class GameFactory : IGameFactory
    {
        private GameObject _heroGameObject;
        private readonly IStaticDataService _staticData;
        private readonly IAssets _assets;
        private readonly IProgressWatchers _progressWatchers;
        private readonly IPersistentProgressService _persistent;

        public DiContainer SceneContextContainer { get; set; }

        public GameFactory(IProgressWatchers progressWatchers, IStaticDataService staticData, IAssets assets, IPersistentProgressService persistent)
        {
            _progressWatchers = progressWatchers;
            _staticData = staticData;
            _assets = assets;
            _persistent = persistent;
        }

        public async void CreateTransferLevelTrigger(string currentScene)
        {
            LevelStaticData levelData = _staticData.ForLevel(currentScene);

            GameObject levelTransferTriggerObject = await InstantiateRegisteredAsync(AssetAddress.TransferLevelTrigger,
                levelData.LevelTransferTrigger.Position);
            
            levelTransferTriggerObject.GetComponent<LevelTransferTrigger>().Configure(levelData.NextSceneKey);
            levelTransferTriggerObject.GetComponent<BoxCollider>().size = levelData.LevelTransferTrigger.ColliderSize;
        }

        public async void CreateSaveTrigger(string currentScene)
        {
            LevelStaticData levelData = _staticData.ForLevel(currentScene);

            GameObject saveTriggerObject = await InstantiateRegisteredAsync(AssetAddress.SaveTrigger, levelData.SaveTrigger.Position);
            
            saveTriggerObject.GetComponent<BoxCollider>().size = levelData.SaveTrigger.ColliderSize;
        }

        public async Task<GameObject> CreateHero(Vector3 at)
        {
            _heroGameObject = await InstantiateRegisteredAsync(AssetAddress.Hero, at);
            Bind_HeroService();
            return _heroGameObject;
        }

        public async Task<GameObject> CreateHud()
        {
            GameObject hud = await InstantiateRegisteredAsync(AssetAddress.Hud);
            hud.GetComponentInChildren<MoneyCounter>().Construct(_persistent.Progress.Money);
            return hud;
        }


        public async Task<GameObject> CreateMonster(MonsterTypeId monsterTypeId, Transform parent)
        {
            MonsterStaticData monsterData = _staticData.ForMonster(monsterTypeId);

            GameObject monster = await InstantiateRegisteredAsync(monsterData.PrefabReference, parent);
            ConfigureMonsterLoot(monster.GetComponentInChildren<LootSpawner>(), monsterData);
            ConfigureMonsterStats(monster.GetComponent<EnemyStatsFacade>(), monsterData);
            InitMonsterUI(monster.GetComponent<ActorUI>(), monster.GetComponent<IEnemyHealth>());

            return monster;
        }

        public async Task<LootPiece> CreateLoot(Vector3 at)
        {
            GameObject loot = await InstantiateRegisteredAsync(AssetAddress.Loot, at);
            return loot.GetComponent<LootPiece>();
        }

        public async Task<SpawnPoint> CreateSpawner(Vector3 at, string uniqueId, MonsterTypeId monsterTypeId)
        {
            GameObject spawnerObject = await InstantiateRegisteredAsync(AssetAddress.SpawnPoint, at);

            var spawnPoint = spawnerObject.GetComponent<SpawnPoint>();
            spawnPoint.Configure(monsterTypeId, uniqueId);
            return spawnPoint;
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
            _assets.CleanUp();
        }

        private async Task<GameObject> InstantiateRegisteredAsync(string address, Vector3 at)
        {
            GameObject prefab = await _assets.LoadSingle<GameObject>(address);
            GameObject gameObject = SceneContextContainer.InstantiatePrefab(prefab, at, Quaternion.identity, null);
            _progressWatchers.Register(gameObject);
            return gameObject;
        }

        private async Task<GameObject> InstantiateRegisteredAsync(AssetReferenceGameObject reference)
        {
            GameObject prefab = await _assets.LoadSingle<GameObject>(reference);
            GameObject gameObject = SceneContextContainer.InstantiatePrefab(prefab);
            _progressWatchers.Register(gameObject);
            return gameObject;
        }
        private async Task<GameObject> InstantiateRegisteredAsync(AssetReferenceGameObject reference, Transform under)
        {
            GameObject prefab = await _assets.LoadSingle<GameObject>(reference);
            GameObject gameObject = SceneContextContainer.InstantiatePrefab(prefab, under);
            _progressWatchers.Register(gameObject);
            return gameObject;
        }

        private async Task<GameObject> InstantiateRegisteredAsync(string address)
        {
            GameObject prefab = await _assets.LoadSingle<GameObject>(address);
            GameObject gameObject = SceneContextContainer.InstantiatePrefab(prefab);
            _progressWatchers.Register(gameObject);
            return gameObject;
        }

        private void Bind_HeroService()
        {
            SceneContextContainer
                .Bind<HeroService>()
                .FromInstance(_heroGameObject.GetComponent<HeroService>())
                .AsSingle()
                .NonLazy();
        }

        private static void ConfigureMonsterLoot(LootSpawner lootSpawner, MonsterStaticData monsterData) => 
            lootSpawner.SetLoot(monsterData.Loot.MaxValue, monsterData.Loot.MinValue);

        private void ConfigureMonsterStats(EnemyStatsFacade to, MonsterStaticData from) => 
            to.Construct(@from);


        private static void InitMonsterUI(ActorUI ui, IEnemyHealth health)
        {
            ui.Construct(health);
        }
        
    }
}