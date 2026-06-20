using System;

namespace Equipment
{
    public interface IShipModules : IEquipmentBattery<IModule, ModuleType>
    {
        event Action<IModule> OnModuleEquipped;
        event Action<IModule> OnModuleUnequip;

    }
}
