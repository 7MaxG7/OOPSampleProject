using System.Collections.Generic;
using Equipment.Data;

namespace Ships
{
    public sealed class ShipConfiguration
    {
        public ShipType ShipType { get; }
        public int WeaponSlotsAmount { get; }
        public int ModuleSlotsAmount { get; }
        public Dictionary<int, WeaponType> WeaponTypes { get; } = new();
        public Dictionary<int, ModuleType> ModuleTypes { get; } = new();

        public ShipConfiguration(ShipConfig shipConfig)
        {
            ShipType = shipConfig.ShipType;
            WeaponSlotsAmount = shipConfig.WeaponSlotsAmount;
            ModuleSlotsAmount = shipConfig.ModuleSlotsAmount;
        }
    }
}
