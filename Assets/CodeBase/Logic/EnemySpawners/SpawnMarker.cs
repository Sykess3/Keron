using CodeBase.StaticData;
using UnityEngine;

namespace CodeBase.Logic.EnemySpawners
{
    [RequireComponent(typeof(UniqueId))]
    public class SpawnMarker : MonoBehaviour
    {
        [SerializeField] private MonsterTypeId monsterTypeId;

        public MonsterTypeId Type() => 
            monsterTypeId;
    }
}