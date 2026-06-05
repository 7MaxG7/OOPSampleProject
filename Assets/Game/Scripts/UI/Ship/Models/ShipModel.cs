using System;
using System.Collections.Generic;
using Equipment.Data;
using Ships;
using Ships.Data;

namespace UI.Ship.Models
{
    public sealed class ShipModel
    {
        public event Action<int, WeaponType> OnWeaponChange;
        public event Action<int, ModuleType> OnModuleChange;
        
        public ShipType ShipType { get; }
        public Dictionary<int, WeaponType> WeaponTypes { get; } = new();
        public Dictionary<int, ModuleType> ModuleTypes { get; } = new();

        private readonly int _weaponSlotsAmount;
        private readonly int _moduleSlotsAmount;
        
        public ShipModel(ShipConfig shipConfig)
        {
            ShipType = shipConfig.ShipType;
            _weaponSlotsAmount = shipConfig.WeaponSlotsAmount;
            _moduleSlotsAmount = shipConfig.ModuleSlotsAmount;
        }

        public void SetWeapon(int slot, WeaponType weaponType)
        {
            if (slot >= _weaponSlotsAmount)
                return;

            WeaponTypes[slot] = weaponType;
            OnWeaponChange?.Invoke(slot, weaponType);
        }

        public void SetModule(int slot, ModuleType moduleType)
        {
            if (slot >= _moduleSlotsAmount)
                return;

            ModuleTypes[slot] = moduleType;
            OnModuleChange?.Invoke(slot, moduleType);
        }
    }
}