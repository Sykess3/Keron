using System;
using CodeBase.Data;
using CodeBase.Infrastructure;
using UnityEngine;

namespace CodeBase.Hero
{
    public class HeroWallet : MonoBehaviour, ISavedProgress
    {
        private Money _money;
        

        public void Collect(int amount)
        {
            _money.Amount += amount;
        }

        public void LoadProgress(PlayerProgress @from) => 
            _money = from.Money;

        public void UpdateProgress(ref PlayerProgress to) => 
            to.Money = _money;
    }
}