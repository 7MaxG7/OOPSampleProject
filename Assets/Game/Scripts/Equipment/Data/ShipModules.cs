using System;
using Cysharp.Threading.Tasks;

namespace Equipment
{
    public sealed class ShipModules : BaseEquipmentBattery<IModule, ModuleType>, IShipModules
    {
        public event Action<IModule> OnModuleEquipped;
        public event Action<IModule> OnModuleUnequip;

        public ShipModules(int amount, IModuleFactory moduleFactory) : base(amount, moduleFactory) { }

        public override async UniTask SetEquipmentAsync(int slotIndex, ModuleType equipType)
        {
            await base.SetEquipmentAsync(slotIndex, equipType);
            Equipments[slotIndex].OnModuleUnequip += InvokeModuleUninstall;
            OnModuleEquipped?.Invoke(Equipments[slotIndex]);
        }

        private void InvokeModuleUninstall(IModule module)
        {
            module.OnModuleUnequip -= InvokeModuleUninstall;
            OnModuleUnequip?.Invoke(module);
        }
    }
}
