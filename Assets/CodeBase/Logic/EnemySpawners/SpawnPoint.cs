using System.Threading.Tasks;
using CodeBase.CustomAttributes;
using CodeBase.Data;
using CodeBase.Enemy;
using CodeBase.Infrastructure;
using CodeBase.Infrastructure.Factory;
using CodeBase.StaticData;
using UnityEngine;

namespace CodeBase.Logic.EnemySpawners
{
    public class SpawnPoint : MonoBehaviour, ISavedProgress
    {
        [ReadOnly] private MonsterTypeId _monsterTypeId;
        private bool _slain;
        private IGameFactory _factory;
        private EnemyDeath _enemyDeath;

        private string _id { get; set; }


        public void Construct(IGameFactory factory, MonsterTypeId monsterTypeId, string uniqueId)
        {
            _factory = factory;
            _monsterTypeId = monsterTypeId;
            _id = uniqueId;
        }

        public void LoadProgress(PlayerProgress @from)
        {
            if (from.KillData.ClearedSpawners.Contains(_id)) 
                _slain = true;
            else
                Spawn();
        }

        public void UpdateProgress(ref PlayerProgress to)
        {
            if (_slain) 
                to.KillData.ClearedSpawners.Add(_id);
        }

        private async void Spawn()
        {
            GameObject monster = await _factory.CreateMonster(_monsterTypeId, transform);
            _enemyDeath = monster.GetComponent<EnemyDeath>();
            _enemyDeath.Happend += Slay;
        }

        private void Slay()
        {
            if (_enemyDeath != null)
                _enemyDeath.Happend -= Slay;
            
            _slain = true;
        }
    }
}