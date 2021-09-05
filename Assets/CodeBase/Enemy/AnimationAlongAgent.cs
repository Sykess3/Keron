using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(EnemyAnimator))]
    public class AnimationAlongAgent : MonoBehaviour
    {
        private const float MinimalSpeed = 0.1f;
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private EnemyAnimator _animator;

        private void Update()
        {
            if(ShouldMove())
                _animator.Move(_agent.velocity.magnitude);
            else
                _animator.StopMovement();
        }

        private bool ShouldMove() => 
            _agent.velocity.magnitude >= MinimalSpeed;
    }
}