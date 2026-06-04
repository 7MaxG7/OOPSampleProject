using Enums;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Configs.Data
{
    [CreateAssetMenu(menuName = "Configs/" + nameof(WeaponData), fileName = nameof(WeaponData))]
    public class WeaponData : ScriptableObject
    {
        [SerializeField] private WeaponType _weaponType;
        [SerializeField] private bool _isActive;
        [SerializeField] private int _damage;
        [SerializeField] private float _cooldown;
        [SerializeField] private AssetReference _prefab;
        [SerializeField] private AssetReference _ammoPrefab;
        [SerializeField] private float _ammoSpeed;
        [SerializeField] private Sprite _icon;

        public WeaponType WeaponType => _weaponType;
        public bool IsActive => _isActive;
        public int Damage => _damage;
        public float Cooldown => _cooldown;
        public AssetReference Prefab => _prefab;
        public AssetReference AmmoPrefab => _ammoPrefab;
        public Sprite Icon => _icon;
        public float AmmoSpeed => _ammoSpeed;
    }
}