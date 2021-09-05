using System.Linq;
using CodeBase.Hero;
using CodeBase.Logic;
using CodeBase.StaticData;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace CodeBase.Enemy
{
    [RequireComponent(typeof(EnemyAnimator))]
    [RequireComponent(typeof(NavMeshAgent))]
    public class Attack : MonoBehaviour
    {
        [SerializeField] private EnemyAnimator _animator;

        [SerializeField] private NavMeshAgent _agent;

        private float _cooldown;
        private float _cleavage;
        private float _effectiveDistance;
        private float _damage;

        private Transform _heroTransform;

        private float _cooldownProgress;
        private int _layerMask;
        private readonly Collider[] _hits = new Collider[1];
        private bool _isAttacking;
        private bool _attackIsEnabled;

        [Inject]
        private void Construct(HeroService heroService) => 
            _heroTransform = heroService.transform;

        public void Configure(AttackStaticData config)
        {
            _cooldown = config.AttackCooldown;
            _cleavage = config.CleavageAttack;
            _effectiveDistance = config.EffectiveDistance;
            _damage = config.Damage;
        }

        private void Awake() => 
            _layerMask = 1 << LayerMask.NameToLayer("Player");

        private void Update()
        {
            UpdateCooldown();

            if (CanAttack()) 
                StartAttack();
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