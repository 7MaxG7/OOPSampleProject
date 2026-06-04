using System;
using Cysharp.Threading.Tasks;
using Enums;
using UnityEngine;

namespace Ships
{
    public interface IShipModules : IAbstractEquipments<IModule, ModuleType>
    {
        event Action<IModule> OnModuleEquiped;
        event Action<IModule> OnModuleUnequip;

        void SetSlots(Transform[] moduleSlots);
        UniTask SetEquipmentAsync(int slot, ModuleType moduleType);
        void SetEquipmentSync(int slot, ModuleType moduleType);
    }
}
