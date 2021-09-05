using CodeBase.StaticData;
using UnityEngine;
using Zenject;

namespace CodeBase.Enemy
{
    public class EnemyStatsFacade : MonoBehaviour
    {
        [SerializeField] private Attack _attack;
        [SerializeField] private Followable _followable;
        [SerializeField] private EnemyHealth _enemyHealth;

        public void Construct(MonsterStaticData config)
        {
            _attack.Configure(config.Attack);
            _followable.Configure(config.Speed);
            _enemyHealth.Consfigure(config.HP);
        }        
    }
}