using UnityEngine;

namespace Ships.Views
{
    public sealed class ShieldView : MonoBehaviour
    {
        private const float VISIBLE_SHIELD_TRESHOLD = .2f;

    
        public void UpdatePower(float currentShield, float maxShield)
        {
            gameObject.SetActive(maxShield > 0 && currentShield / maxShield > VISIBLE_SHIELD_TRESHOLD);
        }
    }
}