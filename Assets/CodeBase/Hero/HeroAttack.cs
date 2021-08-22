using System;
using CodeBase.Data;
using CodeBase.Infrastructure;
using CodeBase.Logic;
using CodeBase.Services;
using CodeBase.Services.Input;
using UnityEngine;

namespace CodeBase.Hero
{
    [RequireComponent(typeof(HeroAnimator))]
    [RequireComponent(typeof(CharacterController))]
    public class HeroAttack : MonoBehaviour, ISavedProgress
    {
        [SerializeField] private HeroAnimator _animator;
        [SerializeField] private CharacterController _characterController;

        private Stats _stats;
        private IInputService _input;
        private int _layerMask;
        private readonly Collider[] _hits = new Collider[3];

        private void Awake()
        {
            _input = AllServices.Container.Single<IInputService>();

            _layerMask = 1 << LayerMask.NameToLayer("Hittable");
        }

        private void Update()
        {
            if (_input.IsAttackButtonUp() && !_animator.IsAttacking)
                _animator.PlayAttack();
        }

        private void OnAttack()
        {
            for (int i = 0; i < Hit(); i++) 
                _hits[i].transform.parent.GetComponent<IHealth>().TakeDamage(_stats.Damage);
            
#if UNITY_EDITOR
            PhysicsDebug.DrawDebugSphere(AttackZoneCenter(), _stats.DamageRadius, 1);
#endif
        }

        public void LoadProgress(PlayerProgress from) =>
            _stats = new Stats(@from.HeroStats.Damage, @from.HeroStats.DamageRadius);

        public void UpdateProgress(ref PlayerProgress to) =>
            to.HeroStats = _stats;

        private int Hit() => 
            Physics.OverlapSphereNonAlloc(AttackZoneCenter(), _stats.DamageRadius, _hits, _layerMask);

        private Vector3 AttackZoneCenter() =>
            new Vector3(transform.position.x, transform.position.y + _characterController.center.y / 5,
                transform.position.z) + transform.forward;
    }
}