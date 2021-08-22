using System;
using UnityEngine;

namespace CodeBase.Enemy
{
    [RequireComponent(typeof(Collider))]
    public class TriggerObserver : MonoBehaviour
    {
        public event Action<Collider> Entered;
        public event Action<Collider> Exited;
        private void OnTriggerEnter(Collider other) => 
            Entered?.Invoke(other);

        private void OnTriggerExit(Collider other) => 
            Exited?.Invoke(other);
    }
}