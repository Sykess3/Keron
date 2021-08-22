using System;
using System.Collections;
using UnityEngine;

namespace CodeBase.Enemy
{
    public class Aggro : MonoBehaviour
    {
        [SerializeField] private Followable _follower;
        [SerializeField] private TriggerObserver _trigger;
        [Tooltip("The time during which the NPC continues the next player after leaving the aggro zone")]
        [SerializeField] private float _cooldown;

        private WaitForSeconds _waitForCooldownEnd;
        private Coroutine _followRoutine;
        private bool _hasTarget;

        private void Start() =>
            _waitForCooldownEnd = new WaitForSeconds(_cooldown);

        private void OnEnable()
        {
            _trigger.Entered += OnTriggerEntered;
            _trigger.Exited += OnTriggerExited;

            TurnOffFollowing();
        }

        private void OnDisable()
        {
            _trigger.Entered -= OnTriggerEntered;
            _trigger.Exited -= OnTriggerExited;
        }

        private void OnTriggerEntered(Collider obj)
        {
            if (!_hasTarget)
            {
                _hasTarget = true;
                StopFollowRoutine();

                TurnOnFollowing(obj.transform);
            }
        }

        private void OnTriggerExited(Collider obj)
        {
            if (_hasTarget)
            {
                _hasTarget = false;
                _followRoutine = StartCoroutine(TurnOffFollowingAfterCooldown());
            }
        }

        private void StopFollowRoutine()
        {
            if (_followRoutine != null)
            {
                StopCoroutine(_followRoutine);
                _followRoutine = null;
            }
        }

        private IEnumerator TurnOffFollowingAfterCooldown()
        {
            yield return _waitForCooldownEnd;
            TurnOffFollowing();
        }

        private void TurnOffFollowing() =>
            _follower.enabled = false;

        private void TurnOnFollowing(Transform by) =>
            _follower.enabled = true;
    }
}