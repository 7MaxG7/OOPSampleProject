using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Infrastructure;
using Sounds;
using Zenject;
using Object = UnityEngine.Object;

namespace Ships
{
    public sealed class ShipsInitializer : IShipsInitializer
    {
        private readonly IShipsFactory _shipsFactory;
        private readonly ISoundService _soundService;
        private readonly IShipConfigurator _shipConfigurator;

        [Inject]
        public ShipsInitializer(IShipsFactory shipsFactory, IShipConfigurator shipConfigurator, ISoundService soundService,
            ICleaner cleaner)
        {
            _shipsFactory = shipsFactory;
            _soundService = soundService;
            _shipConfigurator = shipConfigurator;
            cleaner.AddCleanable(this);
        }

        public void CleanUp()
        {
            foreach (var ship in _shipConfigurator.Ships.Values)
            {
                ship.WeaponBattery.OnShoot -= _soundService.PlayShoot;
                ship.CleanUp();
            }
        }

        public async UniTask CreateShipsAsync()
        {
            var spawnLocations = Object.FindObjectsOfType<ShipSpawnerMarker>()
                .ToDictionary(data => data.OpponentId, data => (data.transform.position, data.transform.rotation));
            
            foreach (var (opponentId, configuration) in _shipConfigurator.ShipConfigurations)
            {
                var location = spawnLocations.GetValueOrDefault(opponentId);
                var ship = await _shipsFactory.CreateShipAsync(configuration.ShipType, location.position, location.rotation);
                foreach (var (slotIndex, weaponType) in configuration.WeaponTypes)
                    await ship.WeaponBattery.SetEquipmentAsync(slotIndex, weaponType);
                foreach (var (slotIndex, moduleType) in configuration.ModuleTypes)
                    await ship.ShipModules.SetEquipmentAsync(slotIndex, moduleType);
                _shipConfigurator.RegisterShip(opponentId, ship);

                ship.WeaponBattery.OnShoot += _soundService.PlayShoot;
            }
        }
    }
}