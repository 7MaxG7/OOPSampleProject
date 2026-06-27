using Equipment;
using UnityEngine;

namespace Ships
{
    public class WeaponCooldownUpgradeHandler : IUpgradeHandler
    {
        public void Upgrade(IShip ship, IModule module)
        {
            var weapons = UpgradeWeapons(ship.WeaponBattery, module);
            ship.SetWeapons(weapons);
        }

        public void Downgrade(IShip ship, IModule module)
        {
            var weapons = DowngradeWeapons(ship.WeaponBattery, module);
            if (ship.WeaponBattery != weapons)
                ship.SetWeapons(weapons);
        }

        private IWeaponBattery UpgradeWeapons(IWeaponBattery currentWeaponBattery, IModule module)
            => new UpgradedWeaponsBattery(currentWeaponBattery, module);

        private IWeaponBattery DowngradeWeapons(IWeaponBattery weaponBattery, IModule module)
        {
            if (weaponBattery is not IDowngradable<IWeaponBattery> upgradedWeaponBattery)
            {
                Debug.LogError($"{this}: weapons cannot be downgraded");
                return weaponBattery;
            }

            return upgradedWeaponBattery.Downgrade(module);
        }
    }
}