using System;
using Ships;

namespace Equipment
{
    public interface IModule : IEquipment
    {
        event Action<IModule> OnUnequip;
        
        ModuleType ModuleType { get; }
        BuffParamType BuffParamType { get; }

        void UpdateParams(UpgradedWeaponsBattery weaponsBattery);
        void UpdateParams(UpgradedHealth health);
    }
}