using System;
using Equipment;

namespace Ships
{
    public sealed class Ship : IShip
    {
        public event Action<IShip> OnDied;

        public IHealth Health { get; private set; }
        public IWeaponBattery WeaponBattery { get; private set; }
        public IShipModules ShipModules { get; private set; }
        public string Name { get; }

        private bool IsDead => Health.CurrentHp <= 0;
        private readonly IShipUpgrader _shipUpgrader;

        public Ship(ShipType shipType, IHealth health, IWeaponBattery weaponBattery, IShipModules shipModules,
            IShipUpgrader shipUpgrader)
        {
            _shipUpgrader = shipUpgrader;
            SetHealth(health);
            SetWeapons(weaponBattery);
            SetModules(shipModules);
            Name = shipType.ToString();
        }

        public void CleanUp() 
        {
            ShipModules.OnModuleEquipped -= UpgradeShip;
            ShipModules.OnModuleUnequip -= DowngradeShip;
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

        private void SetModules(IShipModules shipModules)
        {
            ShipModules = shipModules;
            ShipModules.OnModuleEquipped += UpgradeShip;
            ShipModules.OnModuleUnequip += DowngradeShip;
        }

        private void UpgradeShip(IModule module)
            => _shipUpgrader.Upgrade(this, module);

        private void DowngradeShip(IModule module)
            => _shipUpgrader.Downgrade(this, module);
    }
}