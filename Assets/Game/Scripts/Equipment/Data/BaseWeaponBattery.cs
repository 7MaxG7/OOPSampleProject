using System;
using Cysharp.Threading.Tasks;
using Ships;

namespace Equipment
{
    public abstract class BaseWeaponBattery : BaseEquipmentBattery<IWeapon, WeaponType>, IWeaponBattery
    {
        public event Action<WeaponType> OnShoot;

        public float ReloadRate { get; protected set; }

        private IShip _owner;
        private bool _isActive;


        protected BaseWeaponBattery(IWeaponBattery baseWeaponBattery) : base(baseWeaponBattery) { }

        protected BaseWeaponBattery(int amount, IWeaponFactory weaponFactory) : base(amount, weaponFactory) { }

        public void OnUpdate(float deltaTime)
        {
            if (!_isActive)
                return;

            var deltaCooldown = deltaTime / ReloadRate;
            foreach (var weapon in Equipments.Values)
                if (weapon.IsReady)
                {
                    weapon.ShootAsync().Forget();
                    OnShoot?.Invoke(weapon.WeaponType);
                }
                else
                    weapon.ReduceCooldown(deltaCooldown);
        }
        
        public void Init(IShip owner)
        {
            _owner = owner;
        }

        public override async UniTask SetEquipmentAsync(int slotIndex, WeaponType equipType)
        {
            await base.SetEquipmentAsync(slotIndex, equipType);
            Equipments[slotIndex].Init(_owner);
        }

        public void ToggleShooting(bool isActive)
        {
            _isActive = isActive;
            foreach (var weapon in Equipments.Values)
                weapon.Reload();
        }
    }
}