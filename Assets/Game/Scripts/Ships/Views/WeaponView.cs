using UnityEngine;

namespace Ships
{
    public sealed class WeaponView : MonoBehaviour
    {
        [SerializeField] private Transform _barrel;

        public Transform Barrel => _barrel;
        public float AmmoSpeed { get; private set; }

        public void Init(float ammoSpeed)
            => AmmoSpeed = ammoSpeed;
    }
}