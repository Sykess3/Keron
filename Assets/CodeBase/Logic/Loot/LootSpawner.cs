using System;
using CodeBase.Data;
using CodeBase.Enemy;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services.Randomizer;
using UnityEngine;

namespace CodeBase.Logic.Loot
{
    public class LootSpawner : MonoBehaviour
    {
        [SerializeField] private EnemyDeath _death;
        private IGameFactory _factory;
        private IRandomizer _randomizer;
        private int _minLoot;
        private int _maxLoot;

        public void Construct(IGameFactory factory, IRandomizer randomizer)
        {
            _factory = factory;
            _randomizer = randomizer;
        }

        public void SetLoot(int minLoot, int maxLoot)
        {
            _minLoot = minLoot;
            _maxLoot = maxLoot;
        }

        private void Start()
        {
            _death.Happend += Spawn;
        }

        private void OnDestroy()
        {
            _death.Happend -= Spawn;
        }

        private async void Spawn()
        {
            LootPiece loot = await _factory.CreateLoot(at: transform.position);
            loot.Initialize(lootData: GenerateLoot());
        }

        private LootData GenerateLoot()
        {
            return new LootData()
            {
                Money = new Money()
                {
                    Amount = _randomizer.Next(_minLoot, _maxLoot)
                }
            };
        }
    }
}