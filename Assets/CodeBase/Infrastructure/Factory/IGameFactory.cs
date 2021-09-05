using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.Logic.EnemySpawners;
using CodeBase.Logic.Loot;
using CodeBase.Services;
using CodeBase.StaticData;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure.Factory
{
    public interface IGameFactory 
    {
        Task<GameObject> CreateHero(Vector3 at);
        Task<GameObject> CreateHud();
        
        void CleanUp();
        Task<GameObject> CreateMonster(MonsterTypeId monsterTypeId, Transform parent);
        Task<LootPiece> CreateLoot(Vector3 at);
        Task<SpawnPoint> CreateSpawner(Vector3 at, string uniqueId, MonsterTypeId monsterTypeId);
        void CreateTransferLevelTrigger(string currentScene);
        void CreateSaveTrigger(string currentScene);
        void WarmUp();
        DiContainer SceneContextContainer { get; set; }
    }
}