using System.Collections.Generic;
using CodeBase.Logic.Loot;
using UnityEngine;

namespace CodeBase.Data
{
    [System.Serializable]
    public class WorldData
    {
        public PositionOnLevel PositionOnLevel;
        public NonPickedUpLoot NonPickedUpLoot;
        public WorldData(string initialLevel)
        {
            PositionOnLevel = new PositionOnLevel(level: initialLevel);
            NonPickedUpLoot = new NonPickedUpLoot();
        }
    }
}