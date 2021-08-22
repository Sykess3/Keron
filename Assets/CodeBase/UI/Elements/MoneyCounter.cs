using CodeBase.Data;
using CodeBase.Hero;
using TMPro;
using UnityEngine;

namespace CodeBase.UI.Elements
{
    public class MoneyCounter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _counter;
        private Money _money;

        public void Construct(Money money)
        {
            _money = money;

            _money.Changed += UpdateCounter;
            UpdateCounter();
        }
        
        private void OnDestroy()
        {
            _money.Changed -= UpdateCounter;
        }

        private void UpdateCounter() => 
            _counter.text = $"{_money.Amount}";
    }
}