using System;
using CodeBase.Hero;
using CodeBase.Infrastructure.GameStates;
using UnityEngine;

namespace CodeBase.Logic.SaveLoad
{
    [RequireComponent(typeof(BoxCollider))]
    public class LevelTransferTrigger : MonoBehaviour
    {
        [SerializeField] private BoxCollider _collider;
        private IGameStateMachine _stateMachine;
        private string _nextLevel;

        public void Construct(IGameStateMachine stateMachine, string nextLevel)
        {
            _stateMachine = stateMachine;
            _nextLevel = nextLevel;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out HeroMovement heroMovement))
                _stateMachine.Enter<LoadLevelState, string>(_nextLevel);
        }

        private void OnDrawGizmos()
        {
            if (_collider == null)
                return;

            Gizmos.color = new Color32(30, 200, 30, 130);

            Gizmos.DrawCube(transform.position + _collider.center, _collider.size);
        }
    }
}