using System;

namespace Equipment
{
    public abstract class BaseWeaponBattery : BaseEquipmentBattery<IWeapon, WeaponType>, IWeaponBattery
    {
        public event Action<WeaponType> OnShoot;

        public float ReloadRate { get; protected set; }

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
                    weapon.Shoot();
                    OnShoot?.Invoke(weapon.WeaponType);
                }
                else
                    weapon.ReduceCooldown(deltaCooldown);
        }

        public override void SetEquipment(int slotIndex, WeaponType equipType)
        {
            base.SetEquipment(slotIndex, equipType);
            Equipments[slotIndex].Init(Owner);
        }

        public void ToggleShooting(bool isActive)
        {
            _isActive = isActive;
            foreach (var weapon in Equipments.Values)
                weapon.Reload();
        }
    }
}