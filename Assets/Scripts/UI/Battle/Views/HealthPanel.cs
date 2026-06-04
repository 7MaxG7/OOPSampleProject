using Enums;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.Battle.Views
{
    public sealed class HealthPanel : MonoBehaviour
    {
        [SerializeField] private OpponentId _opponent;
        [SerializeField] private Slider _hpSlider;
        [SerializeField] private Slider _shieldSlider;

        public OpponentId OpponentId => _opponent;

        
        public void SetCurrentHp(float currentHp, float maxHp) 
            => _hpSlider.value = ModifyToSliderValue(currentHp, maxHp);

        public void SetCurrentShield(float currentShield, float maxShield)
            => _shieldSlider.value = ModifyToSliderValue(currentShield, maxShield);

        private float ModifyToSliderValue(float value, float maxValue)
        {
            if (maxValue <= 0)
                return 0;
            
            if (value < 0)
                value = 0;
            return value / maxValue;
        }
    }
}