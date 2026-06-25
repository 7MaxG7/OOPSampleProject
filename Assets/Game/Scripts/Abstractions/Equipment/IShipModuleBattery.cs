using System;

namespace Equipment
{
    public interface IShipModuleBattery : IEquipmentBattery<IModule, ModuleType>
    {
        event Action<IModule> OnModuleEquipped;
        event Action<IModule> OnModuleUnequip;
    }
}