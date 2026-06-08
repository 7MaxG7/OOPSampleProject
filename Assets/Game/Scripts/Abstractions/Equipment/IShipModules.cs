using System;
using Equipment.Data;

namespace Equipment
{
    public interface IShipModules : IEquipments<IModule, ModuleType>
    {
        event Action<IModule> OnModuleEquipped;
        event Action<IModule> OnModuleUnequip;

    }
}
