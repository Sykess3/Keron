using System.Linq;
using CodeBase.Hero;
using CodeBase.Infrastructure.Factory;
using CodeBase.Logic;
using CodeBase.Services;
using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.Enemy
{
    [RequireComponent(typeof(EnemyAnimator))]
    [RequireComponent(typeof(NavMeshAgent))]
    public class Attack : MonoBehaviour
    {
        [SerializeField] private EnemyAnimator _animator;

        [SerializeField] private NavMeshAgent _agent;

        [Header("Load from static data")]
        [SerializeField] private float _cooldown;
        [SerializeField] private float _cleavage;
        [SerializeField] private float _effectiveDistance;
        [SerializeField] private float _damage;

        private Transform _heroTransform;

        private float _cooldownProgress;
        private int _layerMask;
        private readonly Collider[] _hits = new Collider[1];
        private bool _isAttacking;
        private bool _attackIsEnabled;

        public void Construct(Transform heroTransform, float damage, float cooldown, float cleavage, float effectiveDistance)
        {
            _heroTransform = heroTransform;
            _damage = damage;
            _cooldown = cooldown;
            _cleavage = cleavage;
            _effectiveDistance = effectiveDistance;
        }

        private void Awake()
        {
            _layerMask = 1 << LayerMask.NameToLayer("Player");
        }

        private void Update()
        {
            UpdateCooldown();

            if (CanAttack())
            {
                StartAttack();
            }
        }

        private void OnAttackEnd()
        {
            _isAttacking = false;
            _cooldownProgress = _cooldown;
        }

        private void OnAttack()
        {
            if (TryHit(out Collider hit))
            {
                hit.GetComponent<IHealth>().TakeDamage(_damage);

#if UNITY_EDITOR
                PhysicsDebug.DrawDebugSphere(AttackZoneCenter(), _cleavage, 1);
#endif
            }
        }

        public void Enable() =>
            _attackIsEnabled = true;

        public void Disable() =>
            _attackIsEnabled = false;

        private bool TryHit(out Collider hit)
        {
            int hitsCount = Physics.OverlapSphereNonAlloc(AttackZoneCenter(), _cleavage, _hits, _layerMask);
            hit = _hits.FirstOrDefault();
            return hitsCount > 0;
        }

        private void UpdateCooldown()
        {
            if (!IsCooldownOver()) 
                _cooldownProgress -= Time.deltaTime;
        }

        private void StartAttack()
        {
            transform.LookAt(_heroTransform);
            _animator.PlayAttack();

            _isAttacking = true;
        }

        private Vector3 AttackZoneCenter() =>
            new Vector3(transform.position.x, transform.position.y + _agent.height / 5, transform.position.z) +
            transform.forward * _effectiveDistance;

        private bool CanAttack() =>
            _attackIsEnabled && !_isAttacking && IsCooldownOver();

        private bool IsCooldownOver() =>
            _cooldownProgress < 0;
        
        private void OnDrawGizmos()
        {
            if (_agent != null) 
                Gizmos.DrawWireSphere(AttackZoneCenter(), _cleavage);
        }
    }
}