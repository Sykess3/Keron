using System;
using UnityEngine;

namespace CodeBase.Enemy
{
    public abstract class Followable : MonoBehaviour
    {
        [Header("Init from static data")]
        [SerializeField]
        protected float Speed;

        protected Transform Target;

        public void Construct(Transform heroTransform, float speed)
        {
            Target = heroTransform;
            Speed = speed;
        }
    }
}