using UnityEngine;

namespace Ships
{
    public sealed class UpgradedWeaponsBattery : AbstractWeaponBattery, IDowngradable<IWeaponBattery>
    {
        private IWeaponBattery _baseWeaponBattery;
        private readonly IModule _module;

        
        public UpgradedWeaponsBattery(IWeaponBattery baseWeaponBattery, IModule module) : base(baseWeaponBattery)
        {
            _baseWeaponBattery = baseWeaponBattery;
            _module = module;
            UpdateReloadRate();
        }

        public IWeaponBattery Downgrade(IModule module)
        {
            if (_module == module)
                return _baseWeaponBattery;

            if (_baseWeaponBattery is IDowngradable<IWeaponBattery> upgradedWeaponBattery)
                _baseWeaponBattery = upgradedWeaponBattery.Downgrade(module);
            else
                Debug.LogError($"{this}: downgraded module is not found");

            UpdateReloadRate();
            return this;
        }

        private void UpdateReloadRate()
        {
            ReloadRate = _module.IsReloadRelativeReduce
                ? _baseWeaponBattery.ReloadRate * _module.Value
                : _baseWeaponBattery.ReloadRate;
        }
    }
}