using UnityEngine;

namespace Ships
{
    public sealed class WeaponView : MonoBehaviour
    {
        [SerializeField] private Transform _barrel;

        public Transform Barrel => _barrel;
        public float BulletSpeed { get; private set; }

        public void Init(float bulletSpeed)
            => BulletSpeed = bulletSpeed;
    }
}
