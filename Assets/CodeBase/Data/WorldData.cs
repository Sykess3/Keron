using System;

namespace CodeBase.Data
{
    [Serializable]
    public class WorldData
    {
        public PositionOnLevel PositionOnLevel;
        public SerializableDictionary<string, NonPickedUpLoot> NonPickedUpLoot;
        public WorldData(string initialLevel)
        {
            PositionOnLevel = new PositionOnLevel(level: initialLevel);
            NonPickedUpLoot = new SerializableDictionary<string, NonPickedUpLoot>
            {
                {"Graveyard", new NonPickedUpLoot()},
                {"Dungeon", new NonPickedUpLoot()}
            };
        }
    }
}