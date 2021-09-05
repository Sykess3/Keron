using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class AgentMoverToPlayer : Followable
    {
        [SerializeField] private NavMeshAgent _agent;
        private void Start() => 
            _agent.speed = Speed;

        private void Update() => 
            _agent.destination = Target.position;
    }
}