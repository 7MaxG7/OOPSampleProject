using System;
using System.Collections.Generic;
using Equipment.Data;
using Ships;

namespace UI.Ship
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

        public void SetWeapon(int slotIndex, WeaponType weaponType)
        {
            if (slotIndex >= _weaponSlotsAmount)
                return;

            WeaponTypes[slotIndex] = weaponType;
            OnWeaponChange?.Invoke(slotIndex, weaponType);
        }

        public void SetModule(int slotIndex, ModuleType moduleType)
        {
            if (slotIndex >= _moduleSlotsAmount)
                return;

            ModuleTypes[slotIndex] = moduleType;
            OnModuleChange?.Invoke(slotIndex, moduleType);
        }
    }
}