using Equipment;
using UnityEngine;

namespace Ships
{
    public sealed class UpgradedHealth : BaseHealth, IDowngradable<IHealth>
    {
        public IHealth BaseHealth { get; private set; }
        private readonly IModule _module;

        public UpgradedHealth(IHealth baseHealth, IModule module)
        {
            BaseHealth = baseHealth;
            _module = module;
            ShieldRecoveryInterval = baseHealth.ShieldRecoveryInterval;
            _module.UpdateParams(this);
            RestoreHp();
            RestoreShield();
        }

        public IHealth Downgrade(IModule module)
        {
            if (_module == module)
                return BaseHealth;

            if (BaseHealth is IDowngradable<IHealth> upgradedHealth)
                BaseHealth = upgradedHealth.Downgrade(module);
            else
                Debug.LogError($"{this}: base health cannot be downgraded");

            _module.UpdateParams(this);
            return this;
        }
    }
}