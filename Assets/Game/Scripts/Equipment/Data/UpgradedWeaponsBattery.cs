using Ships;
using UnityEngine;

namespace Equipment
{
    public sealed class UpgradedWeaponsBattery : BaseWeaponBattery, IDowngradable<IWeaponBattery>
    {
        public IWeaponBattery BaseWeaponBattery { get; private set; }
        private readonly IModule _module;

        public UpgradedWeaponsBattery(IWeaponBattery baseWeaponBattery, IModule module) : base(baseWeaponBattery)
        {
            BaseWeaponBattery = baseWeaponBattery;
            _module = module;
            _module.UpdateParams(this);
        }

        public IWeaponBattery Downgrade(IModule module)
        {
            if (_module == module)
                return BaseWeaponBattery;

            if (BaseWeaponBattery is IDowngradable<IWeaponBattery> upgradedWeaponBattery)
                BaseWeaponBattery = upgradedWeaponBattery.Downgrade(module);
            else
                Debug.LogError($"{this}: downgraded module is not found");

            _module.UpdateParams(this);
            return this;
        }
    }
}