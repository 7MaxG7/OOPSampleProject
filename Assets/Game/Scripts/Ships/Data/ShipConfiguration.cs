using System.Collections.Generic;
using Equipment;

namespace Ships
{
    public sealed class ShipConfiguration
    {
        public ShipType ShipType { get; }
        public Dictionary<int, WeaponType> WeaponTypes { get; } = new();
        public Dictionary<int, ModuleType> ModuleTypes { get; } = new();

        public ShipConfiguration(ShipConfig shipConfig)
        {
            ShipType = shipConfig.ShipType;
        }

        public void SetWeapon(int slotIndex, WeaponType weaponType)
            => WeaponTypes[slotIndex] = weaponType;

        public void SetModule(int slotIndex, ModuleType moduleType)
            => ModuleTypes[slotIndex] = moduleType;
    }
}
