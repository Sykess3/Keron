using System;

namespace CodeBase.Data
{
    [Serializable]
    public class State
    {
        public float MaxHP;
        public float CurrentHP;

        public State(float maxHP)
        {
            MaxHP = maxHP;
            ResetHP();
        }

        public State(float maxHP, float currentHP)
        {
            MaxHP = maxHP;
            CurrentHP = currentHP;
        }
        public void ResetHP() =>
            CurrentHP = MaxHP;
    }
}