using Cysharp.Threading.Tasks;
using Equipment;
using Infrastructure;
using UnityEngine;
using Utils;
using Zenject;

namespace Ships
{
    public sealed class ShipsFactory : IShipsFactory
    {
        private readonly IAssetsInstantiator _instantiator;
        private readonly IWeaponFactory _weaponFactory;
        private readonly IModuleFactory _moduleFactory;
        private readonly IShipUpgrader _shipUpgrader;
        private readonly IStaticDataService _staticDataService;
        
        private Transform _shipsParent;

        [Inject]
        public ShipsFactory(IAssetsInstantiator instantiator, IWeaponFactory weaponFactory, IModuleFactory moduleFactory
            , IShipUpgrader shipUpgrader, IStaticDataService staticDataService)
        {
            _instantiator = instantiator;
            _weaponFactory = weaponFactory;
            _moduleFactory = moduleFactory;
            _shipUpgrader = shipUpgrader;
            _staticDataService = staticDataService;
        }

        public async UniTask<IShip> CreateShipAsync(ShipType shipType, Vector3 position, Quaternion rotation)
        {
            var config = _staticDataService.GetShip(shipType);
            var health = new Health(config.MaxHp, config.MaxShied, config.ShieldRecovery, config.ShieldRecoveryInterval);
            var weapons = new WeaponBattery(config.WeaponSlotsAmount, _weaponFactory);
            var modules = new ShipModules(config.ModuleSlotsAmount, _moduleFactory);
            
            var ship = new Ship(config.ShipType, health, weapons, modules, _shipUpgrader);
            var shipView = await _instantiator.CreateAsync<ShipView>(config.Prefab, position, rotation, GetShipsContent());
            ship.SetView(shipView);
            return ship;
        }

        private Transform GetShipsContent()
        {
            if (_shipsParent == null)
                _shipsParent = new GameObject(Constants.SHIPS_PARENT_NAME).transform;
            return _shipsParent;
        }
    }
}