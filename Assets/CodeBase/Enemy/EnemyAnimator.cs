using System;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Enemy
{
    [RequireComponent(typeof(Animator))]
    public class EnemyAnimator : MonoBehaviour, IAnimatorStateReader
    {
        
        private Animator _animator;
        public AnimatorState State { get; private set; }
        
        private static readonly int Die = Animator.StringToHash("Die");
        private static readonly int Hit = Animator.StringToHash("Hit");
        private static readonly int Attack = Animator.StringToHash("Attack_1");
        private static readonly int IsMove = Animator.StringToHash("IsMove");
        private static readonly int Speed = Animator.StringToHash("Speed");
        
        private readonly int _idleStateHash = Animator.StringToHash("Idle");
        private readonly int _attackStateHash = Animator.StringToHash("Attack01");
        private readonly int _walkingStateHash = Animator.StringToHash("Movement");
        private readonly int _deathStateHash = Animator.StringToHash("Die");
        private readonly int _getHitStateHash = Animator.StringToHash("GetHit");

        public event Action<AnimatorState> StateEntered;
        public event Action<AnimatorState> StateExited;
        
        private void Awake() => 
            _animator = GetComponent<Animator>();

        public void PlayDie() =>
            _animator.SetTrigger(Die);

        public void PlayHit() =>
            _animator.SetTrigger(Hit);

        public void PlayAttack() =>
            _animator.SetTrigger(Attack);

        public void Move(float speed)
        {
            _animator.SetBool(IsMove, true);
            _animator.SetFloat(Speed, speed);
        }

        public void StopMovement() => 
            _animator.SetBool(IsMove, false);


        void IAnimatorStateReader.EnteredState(int hash)
        {
            State = StateFor(hash);
            StateEntered?.Invoke(State);
        }

        void IAnimatorStateReader.ExitedState(int hash) => 
            StateExited?.Invoke(StateFor(hash));

        private AnimatorState StateFor(int stateHash)
        {
            AnimatorState state;
            if (stateHash == _idleStateHash)
                state = AnimatorState.Idle;
            else if (stateHash == _attackStateHash)
                state = AnimatorState.Attack;
            else if (stateHash == _walkingStateHash)
                state = AnimatorState.Walking;
            else if (stateHash == _deathStateHash)
                state = AnimatorState.Died;
            else if (stateHash == _getHitStateHash)
                state = AnimatorState.GetHit;
            else
                state = AnimatorState.Unknown;
      
            return state;
        }
    }
}