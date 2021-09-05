using System;
using CodeBase.CustomAttributes;
using UnityEngine;

namespace CodeBase.Enemy
{
    [RequireComponent(typeof(EnemyAnimator))]
    public class EnemyHealth : MonoBehaviour, IEnemyHealth
    {
        [SerializeField] private EnemyAnimator _animator;

        private float _current;

        private float _max;

        public float Current => _current;

        public float Max => _max;

        public event Action HealthChanged;

        public void Consfigure(float health)
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