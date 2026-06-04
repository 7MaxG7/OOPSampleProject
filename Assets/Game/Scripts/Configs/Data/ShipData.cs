using Enums;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Configs.Data
{
    [CreateAssetMenu(menuName = "Configs/" + nameof(ShipData), fileName = nameof(ShipData))]
    public class ShipData : ScriptableObject
    {
        [SerializeField] private ShipType _shipType;
        [SerializeField] private float _maxHp;
        [SerializeField] private float _maxShied;
        [SerializeField] private float _shieldRecovery;
        [SerializeField] private float _shieldRecoveryInterval;
        [SerializeField] private int _weaponSlotsAmount;
        [SerializeField] private int _moduleSlotsAmount;
        [SerializeField] private AssetReference _prefab;

        public ShipType ShipType => _shipType;
        public float MaxHp => _maxHp;
        public float MaxShied => _maxShied;
        public float ShieldRecovery => _shieldRecovery;
        public float ShieldRecoveryInterval => _shieldRecoveryInterval;
        public int WeaponSlotsAmount => _weaponSlotsAmount;
        public int ModuleSlotsAmount => _moduleSlotsAmount;
        public AssetReference Prefab => _prefab;
    }
}