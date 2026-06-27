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

        public void SetWeapon(IShip ship, int slotIndex, IWeapon weapon)
            => WeaponTypes[slotIndex] = weapon.WeaponType;

        public void SetModule(IShip ship, int slotIndex, IModule module)
            => ModuleTypes[slotIndex] = module.ModuleType;
    }
}