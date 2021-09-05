using System;

namespace CodeBase.Data
{
    [Serializable]
    public class PlayerProgress
    {
        public WorldData WorldData;
        public State HeroState;
        public Stats HeroStats;
        public KillData KillData;
        public Money Money;
        public PurchaseData PurchaseData;


        public PlayerProgress(WorldData worldData, State heroState, Stats heroStats)
        {
            WorldData = worldData;
            HeroState = heroState;
            HeroStats = heroStats;
            KillData = new KillData();
            Money = new Money();
            PurchaseData = new PurchaseData();
        }
    }
}