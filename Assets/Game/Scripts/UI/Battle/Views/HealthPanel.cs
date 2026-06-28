using Ships;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Battle.Views
{
    public sealed class HealthPanel : MonoBehaviour
    {
        [SerializeField] private OpponentId _opponent;
        [SerializeField] private Slider _hpSlider;
        [SerializeField] private Slider _shieldSlider;

        public OpponentId OpponentId => _opponent;

        public void SetHp((float current, float max) hp) 
            => _hpSlider.value = ModifyToSliderValue(hp.current, hp.max);

        public void SetShield((float current, float max) shield)
            => _shieldSlider.value = ModifyToSliderValue(shield.current, shield.max);

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