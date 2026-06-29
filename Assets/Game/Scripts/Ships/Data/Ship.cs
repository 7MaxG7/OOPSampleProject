using System;
using Equipment;

namespace Ships
{
    public sealed class Ship : IShip
    {
        public event Action<IShip> OnDied;

        public ShipType ShipType { get; }
        public IHealth Health { get; private set; }
        public IWeaponBattery WeaponBattery { get; private set; }
        public IShipModuleBattery ModuleBattery { get; private set; }
        public string Name { get; }

        private bool IsDead => Health.CurrentHp <= 0;
        private readonly IShipUpgrader _shipUpgrader;

        public Ship(ShipType shipType, IHealth health, IWeaponBattery weaponBattery, IShipModuleBattery shipModuleBattery,
            IShipUpgrader shipUpgrader)
        {
            ShipType = shipType;
            _shipUpgrader = shipUpgrader;
            SetHealth(health);
            SetWeapons(weaponBattery);
            SetModules(shipModuleBattery);
            Name = shipType.ToString();
        }

        public void Clean() 
        {
            ModuleBattery.OnModuleEquipped -= UpgradeShip;
            ModuleBattery.OnModuleUnequip -= DowngradeShip;
        }

        public void TakeDamage(int damage)
        {
            if (IsDead)
                return;
            
            Health.TakeDamage(damage);
            if (IsDead)
                OnDied?.Invoke(this);
        }

        public void SetHealth(IHealth health)
            => Health = health;

        public void SetWeapons(IWeaponBattery weaponBattery)
        {
            WeaponBattery = weaponBattery;
            WeaponBattery.Init(this);
        }

        private void SetModules(IShipModuleBattery shipModuleBattery)
        {
            ModuleBattery = shipModuleBattery;
            ModuleBattery.Init(this);
            ModuleBattery.OnModuleEquipped += UpgradeShip;
            ModuleBattery.OnModuleUnequip += DowngradeShip;
        }

        private void UpgradeShip(IModule module)
            => _shipUpgrader.Upgrade(this, module);

        private void DowngradeShip(IModule module)
            => _shipUpgrader.Downgrade(this, module);
    }
}