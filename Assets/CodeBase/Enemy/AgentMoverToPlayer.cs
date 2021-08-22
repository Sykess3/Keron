using System;
using CodeBase.Infrastructure.Factory;
using CodeBase.Services;
using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class AgentMoverToPlayer : Followable
    {
        [Space(20f)]
        [SerializeField] private NavMeshAgent _agent;
        private void Start() => 
            _agent.speed = Speed;

        private void Update()
        {
            if (IsInitializedHeroTransform())
                _agent.destination = Target.position;
        }

        private bool IsInitializedHeroTransform() =>
            Target != null;
    }
}