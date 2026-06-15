using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Equipment;
using Equipment.Data;
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

        public async UniTask<IShip> CreateShipAsync(ShipConfiguration configuration, Vector3 position, Quaternion rotation)
        {
            var ship = await CreateShipAsync(configuration.ShipType, position, rotation);
            await SetWeaponsAsync(ship.WeaponBattery, configuration.WeaponTypes);
            await SetModulesAsync(ship.ShipModules, configuration.ModuleTypes);

            return ship;
        }

        private async UniTask<Ship> CreateShipAsync(ShipType shipType, Vector3 position, Quaternion rotation)
        {
            var shipData = _staticDataService.GetShip(shipType);
            var health = new Health(shipData.MaxHp, shipData.MaxShied, shipData.ShieldRecovery, shipData.ShieldRecoveryInterval);
            var weapons = new WeaponBattery(shipData.WeaponSlotsAmount, _weaponFactory);
            var modules = new ShipModules(shipData.ModuleSlotsAmount, _moduleFactory);
            
            var ship = new Ship(shipData.ShipType, health, weapons, modules, _shipUpgrader);
            var shipView = await _instantiator.CreateAsync<ShipView>(shipData.Prefab, position, rotation, GetShipsContent());
            ship.SetView(shipView);
            return ship;
        }

        private async UniTask SetWeaponsAsync(IWeaponBattery weapons, Dictionary<int, WeaponType> weaponTypes)
        {
            foreach (var slotIndex in weaponTypes.Keys.Where(slotIndex => slotIndex < weapons.MaxEquipmentsAmount))
                await weapons.SetEquipmentAsync(slotIndex, weaponTypes[slotIndex]);
        }

        private async UniTask SetModulesAsync(IShipModules modules, Dictionary<int, ModuleType> moduleTypes)
        {
            foreach (var slotIndex in moduleTypes.Keys.Where(slotIndex => slotIndex < modules.MaxEquipmentsAmount))
                await modules.SetEquipmentAsync(slotIndex, moduleTypes[slotIndex]);
        }

        private Transform GetShipsContent()
        {
            if (_shipsParent == null)
                _shipsParent = new GameObject(Constants.SHIPS_PARENT_NAME).transform;
            return _shipsParent;
        }
    }
}