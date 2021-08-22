using System;
using UnityEngine;

namespace CodeBase.Hero
{
    [RequireComponent(typeof(HeroMovement))]
    [RequireComponent(typeof(HeroHealth))]
    public class HeroDeath : MonoBehaviour
    {
        [SerializeField] private HeroAnimator _animator;
        [SerializeField] private HeroHealth _health;
        [SerializeField] private HeroMovement _movement;
        [SerializeField] private HeroAttack _attack;
        [SerializeField] private Collider _collider;
        [SerializeField] private ParticleSystem _deathFX;
        private bool _isDead;

        public event Action Happend;
        private void Start() => 
            _health.HealthChanged += OnHealthChanged;

        private void OnDestroy()
        {
            _health.HealthChanged -= OnHealthChanged;
        }

        private void OnHealthChanged()
        {
            if (_health.Current <= 0 && !_isDead) 
                Die();
        }


        private void Die()
        {
            _health.HealthChanged -= OnHealthChanged;
            
            _isDead = true;
            DisableAttack();
            DisableMovement();
            DisableCollider();
            
            _animator.PlayDeath();

            CreateDeathFX();
            
            Happend?.Invoke();
        }

        private void DisableCollider()
        {
            _collider.enabled = false;
        }

        private void DisableAttack() => 
            _attack.enabled = false;

        private void DisableMovement() => 
            _movement.enabled = false;

        private void CreateDeathFX() => 
            Instantiate(_deathFX, transform.position, Quaternion.identity);
    }
}