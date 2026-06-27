using Equipment;
using UnityEngine;

namespace Ships
{
    public class HealthUpgradeHandler : IUpgradeHandler
    {
        public void Upgrade(IShip ship, IModule module)
        {
            var health = UpgradeHealth(ship.Health, module);
            ship.SetHealth(health);
        }

        public void Downgrade(IShip ship, IModule module)
        {
            var health = DowngradeHealth(ship.Health, module);
            if (ship.Health != health)
                ship.SetHealth(health);
        }

        private IHealth UpgradeHealth(IHealth currentHealth, IModule module)
            => new UpgradedHealth(currentHealth, module);

        private IHealth DowngradeHealth(IHealth health, IModule module)
        {
            if (health is not IDowngradable<IHealth> upgradedHealth)
            {
                Debug.LogError($"{this}: health cannot be downgraded");
                return health;
            }

            var downgradedHealth = upgradedHealth.Downgrade(module);
            downgradedHealth.RestoreHp();
            downgradedHealth.RestoreShield();
            return downgradedHealth;
        }
    }
}