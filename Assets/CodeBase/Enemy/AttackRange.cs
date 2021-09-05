using UnityEngine;

namespace CodeBase.Enemy
{
    [RequireComponent(typeof(Attack))]
    public class AttackRange : MonoBehaviour
    {
        [SerializeField] private Attack _attack;
        [SerializeField] private TriggerObserver _range;

        private void OnEnable()
        {
            _range.Entered += OnRangeEntered;
            _range.Exited += OnRangeExited;
            
            _attack.Disable();
        }

        private void OnDisable()
        {
            _range.Entered -= OnRangeEntered;
            _range.Exited -= OnRangeExited;
        }

        private void OnRangeExited(Collider obj) => 
            _attack.Disable();

        private void OnRangeEntered(Collider obj) => 
            _attack.Enable();
    }
}