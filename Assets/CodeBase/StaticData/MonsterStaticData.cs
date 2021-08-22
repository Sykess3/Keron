using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Serialization;

namespace CodeBase.StaticData
{
    [CreateAssetMenu(fileName = "New monster", menuName = "StaticData/Monster")]
    public class MonsterStaticData : ScriptableObject
    {
        [FormerlySerializedAs("MonsterType")] public MonsterTypeId monsterTypeId;
        
        [Range(1f, 10f)]
        public float HP;
        
        [Range(0.5f, 10f)]
        public float Speed;

        public AttackStaticData Attack;

        public LootStaticData Loot;
        
        public AssetReferenceGameObject PrefabReference;
    }
}
