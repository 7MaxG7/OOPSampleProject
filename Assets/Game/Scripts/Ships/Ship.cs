using System;
using Enums;
using Services;
using Ships.Views;
using Object = UnityEngine.Object;

namespace Ships
{
    public sealed class Ship : IShip
    {
        public event Action<IShip> OnDied;

        public IHealth Health { get; private set; }
        public IWeaponBattery WeaponBattery { get; private set; }
        public IShipModules ShipModules { get; private set; }
        public string Name { get; }

        public ShipView ShipView { get; private set; }
        private ShipType ShipType { get; }
        private bool IsDead => Health.CurrentHp <= 0;
        private readonly IShipUpgrader _shipUpgrader;


        public Ship(ShipType shipType, IHealth health, IWeaponBattery weaponBattery, IShipModules shipModules,
            IShipUpgrader shipUpgrader)
        {
            _shipUpgrader = shipUpgrader;
            ShipType = shipType;
            SetHealth(health);
            SetWeapons(weaponBattery);
            SetModules(shipModules);
            Name = ShipType.ToString();
        }

        public void CleanUp() 
            => SceneCleanUp();

        public void SceneCleanUp()
        {
            ShipModules.OnModuleEquiped -= UpgradeShip;
            ShipModules.OnModuleUnequip -= DowngradeShip;
            Health.OnShieldChanged -= ShipView.Shield.UpdatePower;
        }

        public void SetView(ShipView shipView)
        {
            ShipView = shipView;
            WeaponBattery.SetSlots(ShipView.WeaponSlots);
            ShipModules.SetSlots(ShipView.ModuleSlots);
        }

        public void PrepareToBattle()
        {
            Health.OnShieldChanged += ShipView.Shield.UpdatePower;
            ShipView.Shield.UpdatePower(Health.CurrentShield, Health.MaxShield);
        }

        public void TakeDamage(int damage)
        {
            if (IsDead)
                return;
            
            Health.TakeDamage(damage);
            if (IsDead)
                OnDied?.Invoke(this);
        }

        public void Kill()
        {
            Object.Destroy(ShipView.gameObject);
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
            ShipModules.OnModuleEquiped += UpgradeShip;
            ShipModules.OnModuleUnequip += DowngradeShip;
        }

        private void UpgradeShip(IModule module)
            => _shipUpgrader.Upgrade(this, module);

        private void DowngradeShip(IModule module)
            => _shipUpgrader.Downgrade(this, module);
    }
}