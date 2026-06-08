using UnityEngine;

namespace Ships.Views
{
    public sealed class ShieldView : MonoBehaviour
    {
        [Tooltip("Порог, выше которого щит становится видимым")]
        [SerializeField] private float visibleShieldThreshold = .2f;
    
        public void UpdatePower(float currentShield, float maxShield)
            => gameObject.SetActive(maxShield > 0 && currentShield / maxShield > visibleShieldThreshold);
    }
}