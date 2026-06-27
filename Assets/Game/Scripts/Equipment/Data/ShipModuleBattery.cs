using System;

namespace Equipment
{
    public sealed class ShipModuleBattery : BaseEquipmentBattery<IModule, ModuleType>, IShipModuleBattery
    {
        public event Action<IModule> OnModuleEquipped;
        public event Action<IModule> OnModuleUnequip;

        public ShipModuleBattery(int amount, IModuleFactory moduleFactory) : base(amount, moduleFactory) { }

        public override void SetEquipment(int slotIndex, ModuleType equipType)
        {
            base.SetEquipment(slotIndex, equipType);
            Equipments[slotIndex].OnUnequip += InvokeModuleUninstall;
            OnModuleEquipped?.Invoke(Equipments[slotIndex]);
        }

        private void InvokeModuleUninstall(IModule module)
        {
            module.OnUnequip -= InvokeModuleUninstall;
            OnModuleUnequip?.Invoke(module);
        }
    }
}