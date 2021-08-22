using System;
using UnityEngine;

namespace CodeBase.StaticData
{
    [Serializable]
    public class EnemySpawnerData
    {
        public MonsterTypeId MonsterTypeId;
        public string UniqueId;
        public Vector3 Position;

        public EnemySpawnerData(string id, MonsterTypeId type, Vector3 position)
        {
            MonsterTypeId = type;
            UniqueId = id;
            Position = position;
        }
    }
}