using System;
using UnityEngine;

namespace CodeBase.Data
{
    [Serializable]
    public class Money
    {
        [SerializeField] private int amount;
        public event Action Changed;
        
        public int Amount
        {
            set
            {
                amount = value;
                Changed?.Invoke();
            }
            get => amount;
        }

        public Money() { }

        public Money(int amount)
        {
            Amount = amount;
        }
    }
}