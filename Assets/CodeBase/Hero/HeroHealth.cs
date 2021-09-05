using System;
using CodeBase.Data;
using CodeBase.Infrastructure;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Hero
{
    [RequireComponent(typeof(HeroAnimator))]
    public class HeroHealth : MonoBehaviour, ISavedProgressCleanable, IHealth
    {
        [SerializeField] private HeroAnimator _animator;

        private State _state;
        
        public event Action HealthChanged;
        public float Max =>
            _state.MaxHP;
        public float Current =>
            _state.CurrentHP;

        public void TakeDamage(float amount)
        {
            if (Current <= 0)
                return;

            _state.CurrentHP -= amount;
            _animator.PlayHit();
            HealthChanged?.Invoke();
        }

        public void LoadProgress(PlayerProgress from)
        {
            State heroState = from.HeroState;
            _state = new State(heroState.MaxHP, heroState.CurrentHP);
            HealthChanged?.Invoke();
        }

        public void UpdateProgress(ref PlayerProgress to)
        {
            to.HeroState = _state;
        }
    }
}