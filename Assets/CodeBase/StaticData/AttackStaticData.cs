using UnityEngine;

namespace CodeBase.StaticData
{
    [System.Serializable]
    public class AttackStaticData
    {
        [Range(0.5f, 1f)]
        public float CleavageAttack;

        [Range(0.5f, 5f)]
        public float AttackCooldown;

        [Range(0.5f, 1f)]
        public float EffectiveDistance;

        [Range(1f, 30f)]
        public float Damage;
    }
}