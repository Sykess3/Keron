using System;
using CodeBase.Data;
using CodeBase.Infrastructure;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Enemy
{
    [RequireComponent(typeof(EnemyAnimator))]
    public class EnemyHealth : MonoBehaviour, IEnemyHealth
    {
        [SerializeField] private EnemyAnimator _animator;

        [SerializeField] private float _current;

        [Header("Load from static data")]
        [SerializeField] private float _max;

        public float Current => _current;

        public float Max => _max;

        public event Action HealthChanged;

        public void Construct(float health)
        {
            _max = health;
            _current = health;
        }

        public void TakeDamage(float amount)
        {
            _current -= amount;
            _animator.PlayHit();
            
            HealthChanged?.Invoke();
        }
        
    }
}