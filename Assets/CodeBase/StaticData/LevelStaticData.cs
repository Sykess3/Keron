using System;
using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.StaticData
{
    [CreateAssetMenu(fileName = "New LevelStaticData", menuName = "StaticData/Level")]
    public class LevelStaticData : ScriptableObject
    {
        public string SceneKey;
        public string NextSceneKey;
        public List<EnemySpawnerData> Spawners;
        public Vector3 InitialPoint;
        public TriggerData LevelTransferTrigger;
        public TriggerData SaveTrigger;

    }

    [Serializable]
    public class TriggerData
    {
        public Vector3 Position;
        public Vector3 ColliderSize;
    }
}