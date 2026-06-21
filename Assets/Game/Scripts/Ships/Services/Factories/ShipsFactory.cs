using Equipment;
using Infrastructure;
using Zenject;

namespace Ships
{
    public sealed class ShipsFactory : IShipsFactory
    {
        private readonly IWeaponFactory _weaponFactory;
        private readonly IModuleFactory _moduleFactory;
        private readonly IShipUpgrader _shipUpgrader;
        private readonly IStaticDataService _staticDataService;

        [Inject]
        public ShipsFactory(IWeaponFactory weaponFactory, IModuleFactory moduleFactory, IShipUpgrader shipUpgrader,
            IStaticDataService staticDataService)
        {
            _weaponFactory = weaponFactory;
            _moduleFactory = moduleFactory;
            _shipUpgrader = shipUpgrader;
            _staticDataService = staticDataService;
        }

        public IShip CreateShip(ShipType shipType)
        {
            var config = _staticDataService.GetShip(shipType);
            var health = new Health(config.MaxHp, config.MaxShied, config.ShieldRecovery, config.ShieldRecoveryInterval);
            var weapons = new WeaponBattery(config.WeaponSlotsAmount, _weaponFactory);
            var modules = new ShipModules(config.ModuleSlotsAmount, _moduleFactory);

            return new Ship(config.ShipType, health, weapons, modules, _shipUpgrader);
        }
    }
}