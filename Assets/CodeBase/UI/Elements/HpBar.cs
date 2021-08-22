using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Elements
{
    public class HpBar : MonoBehaviour
    {
        [SerializeField] private Image _current;
        
        public void SetValue(float current, float max) =>
            _current.fillAmount = current / max;
    }
}