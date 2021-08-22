using System;
using System.Collections;
using UnityEngine;

namespace CodeBase.Enemy
{
    [RequireComponent(typeof(EnemyAnimator))]
    [RequireComponent(typeof(EnemyHealth))]
    [RequireComponent(typeof(Attack))]
    public class EnemyDeath : MonoBehaviour
    {
        [SerializeField] private EnemyAnimator _animator;
        [SerializeField] private EnemyHealth _health;
        [SerializeField] private Followable _following;
        [SerializeField] private Attack _attack;
        [SerializeField] private Aggro _aggro;
        [SerializeField] private Collider _collider;
        [SerializeField] private ParticleSystem _deathFX;
        [Tooltip("In seconds")]
        [SerializeField] private float _destroyTime = 3f;
        
        private bool _isDead;

        public event Action Happend;

        private void Start()
        {
            _health.HealthChanged += OnHealthChanged;
        }

        private void OnHealthChanged()
        {
            if (_health.Current <= 0 && !_isDead)
                Die();
        }

        private void OnDestroy()
        {
            _health.HealthChanged -= OnHealthChanged;
        }

        private void Die()
        {
            _health.HealthChanged -= OnHealthChanged;
            _isDead = true;
            
            DisableAttack();
            DisableAggro();
            DisableMovement();
            DisableCollider();
            
            _animator.PlayDie();

            CreateDeathFX();
            StartCoroutine(DestroyTimer());
            
            Happend?.Invoke();
        }

        private void DisableAggro() => 
            _aggro.enabled = false;

        private void DisableCollider() => 
            _collider.enabled = false;

        private void DisableAttack() => 
            _attack.enabled = false;

        private void DisableMovement() => 
            _following.enabled = false;

        private void CreateDeathFX() =>
            Instantiate(_deathFX, transform.position, Quaternion.identity);

        private IEnumerator DestroyTimer()
        {
            yield return new WaitForSeconds(_destroyTime);
            Destroy(gameObject);
        }
    }
}