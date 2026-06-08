using System;
using Cysharp.Threading.Tasks;

namespace Equipment.Data
{
    public sealed class ShipModules : BaseEquipmentBattery<IModule, ModuleType>, IShipModules
    {
        public event Action<IModule> OnModuleEquipped;
        public event Action<IModule> OnModuleUnequip;

        public ShipModules(int amount, IModuleFactory moduleFactory) : base(amount, moduleFactory) { }

        public override async UniTask SetEquipmentAsync(int index, ModuleType equipType)
        {
            await base.SetEquipmentAsync(index, equipType);
            Equipments[index].OnModuleUnequip += InvokeModuleUninstall;
            OnModuleEquipped?.Invoke(Equipments[index]);
        }

        private void InvokeModuleUninstall(IModule module)
        {
            module.OnModuleUnequip -= InvokeModuleUninstall;
            OnModuleUnequip?.Invoke(module);
        }
    }
}
