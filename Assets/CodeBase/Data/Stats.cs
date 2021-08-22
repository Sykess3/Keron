using System;

namespace CodeBase.Data
{
    [Serializable]
    public class Stats
    {
        public float Damage;
        public float DamageRadius;

        public Stats(float damage, float damageRadius)
        {
            Damage = damage;
            DamageRadius = damageRadius;
        }
    }
}