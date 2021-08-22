using System;

namespace CodeBase.Data
{
    [Serializable]
    public class LootPieceData
    {
        public Vector3Data Position;
        public LootData LootData;

        public LootPieceData(LootData lootData, Vector3Data position)
        {
            LootData = lootData;
            Position = position;
        }
    }
}