using CodeBase.Hero;
using UnityEngine;
using Zenject;

namespace CodeBase.Enemy
{
    public abstract class Followable : MonoBehaviour
    {
        protected float Speed { get; private set; }

        protected Transform Target;

        [Inject]
        private void Construct(HeroService heroService) => 
            Target = heroService.transform;

        public void Configure(float speed) => 
            Speed = speed;
        
    }
}