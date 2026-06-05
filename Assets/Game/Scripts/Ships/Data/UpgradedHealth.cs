using Equipment;
using UnityEngine;

namespace Ships.Data
{
    public sealed class UpgradedHealth : AbstractHealth, IDowngradable<IHealth>
    {
        private IHealth _baseHealth;
        private readonly IModule _module;

        public UpgradedHealth(IHealth baseHealth, IModule module)
        {
            _baseHealth = baseHealth;
            _module = module;
            ShieldRecoveryInterval = baseHealth.ShieldRecoveryInterval;
            UpdateModuledValues();
            RestoreHp();
            RestoreShield();
        }

        public IHealth Downgrade(IModule module)
        {
            if (_module == module)
                return _baseHealth;

            if (_baseHealth is IDowngradable<IHealth> upgradedHealth)
                _baseHealth = upgradedHealth.Downgrade(module);
            else
                Debug.LogError($"{this}: base health cannot be downgraded");

            UpdateModuledValues();
            return this;
        }

        /// <summary>
        /// Updated stats that can be changed by upgrades / downgrades
        /// </summary>
        private void UpdateModuledValues()
        {
            ShieldRecovery = _module.IsShieldRecoveryRelativeSpeedup
                ? _baseHealth.ShieldRecovery * _module.Value
                : _baseHealth.ShieldRecovery;
            MaxHp = _module.IsHpConstantIncrease 
                ? _baseHealth.MaxHp + _module.Value
                : _baseHealth.MaxHp;
            MaxShield =_module.IsShieldConstantIncrease 
                ? _baseHealth.MaxShield + _module.Value
                : _baseHealth.MaxShield;
        }
    }
}