using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Equipment
{
    [CreateAssetMenu(menuName = "Configs/" + nameof(WeaponConfig), fileName = nameof(WeaponConfig))]
    public class WeaponConfig : ScriptableObject
    {
        [SerializeField] private WeaponType _weaponType;
        [SerializeField] private bool _isActive;
        [SerializeField] private int _damage;
        [SerializeField] private float _cooldown;
        [SerializeField] private AssetReference _prefab;
        [SerializeField] private AssetReference _bulletPrefab;
        [SerializeField] private float _bulletSpeed;
        [SerializeField] private Sprite _icon;

        public WeaponType WeaponType => _weaponType;
        public bool IsActive => _isActive;
        public int Damage => _damage;
        public float Cooldown => _cooldown;
        public AssetReference Prefab => _prefab;
        public AssetReference BulletPrefab => _bulletPrefab;
        public Sprite Icon => _icon;
        public float BulletSpeed => _bulletSpeed;
    }
}
