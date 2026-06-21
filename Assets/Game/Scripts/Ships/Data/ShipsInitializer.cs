using System.Collections.Generic;
using System.Linq;
using Battle;
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
        private readonly IShipViewFactory _shipViewFactory;
        private readonly ISoundService _soundService;
        private readonly IShipConfigurator _shipConfigurator;
        private readonly IWinnerDefiner _winnerDefiner;
        private readonly IDamageableIdentifier _damageableIdentifier;
        
        private readonly Dictionary<OpponentId, ShipView> _shipViews = new();

        [Inject]
        public ShipsInitializer(IShipsFactory shipsFactory, IShipViewFactory shipViewFactory, IShipConfigurator shipConfigurator,
            ISoundService soundService, IWinnerDefiner winnerDefiner, IDamageableIdentifier damageableIdentifier, ICleaner cleaner)
        {
            _shipsFactory = shipsFactory;
            _shipViewFactory = shipViewFactory;
            _soundService = soundService;
            _shipConfigurator = shipConfigurator;
            _winnerDefiner = winnerDefiner;
            _damageableIdentifier = damageableIdentifier;
            cleaner.AddCleanable(this);
        }

        public void CleanUp()
        {
            foreach (var (opponentId, ship) in _shipConfigurator.Ships)
            {
                ship.WeaponBattery.OnShoot -= _soundService.PlayShoot;
                ship.OnDied -= DestroyShipView;
                if (_shipViews.TryGetValue(opponentId, out var shipView))
                    ship.Health.OnShieldChanged -= shipView.Shield.UpdatePower;
                ship.CleanUp();
            }

            _shipViews.Clear();
        }

        public void CreateShipsAsync()
        {
            foreach (var (opponentId, configuration) in _shipConfigurator.ShipConfigurations)
            {
                var ship = _shipsFactory.CreateShip(configuration.ShipType);
                _shipConfigurator.RegisterShip(opponentId, ship);
                
                _winnerDefiner.AddShip(ship);
            }
        }

        public async UniTask CreateShipsViewsAsync()
        {
            var spawnLocations = Object.FindObjectsOfType<ShipSpawnerMarker>()
                .ToDictionary(data => data.OpponentId, data => (data.transform.position, data.transform.rotation));

            foreach (var (opponentId, ship) in _shipConfigurator.Ships)
            {
                var configuration = _shipConfigurator.ShipConfigurations[opponentId];
                var location = spawnLocations.GetValueOrDefault(opponentId);
                var shipView = await _shipViewFactory.CreateShipViewAsync(configuration.ShipType, location.position, location.rotation);
                ship.WeaponBattery.SetSlots(shipView.WeaponSlots);
                ship.ShipModules.SetSlots(shipView.ModuleSlots);
                
                // TODO. Move logical part to CreateShipsAsync, when equip views will be separated (before _shipConfigurator.RegisterShip)
                var weaponSlots = configuration.WeaponTypes.Keys.ToArray();
                foreach (var slotIndex in weaponSlots)
                    await ship.WeaponBattery.SetEquipmentAsync(slotIndex, configuration.WeaponTypes[slotIndex]);
                var moduleSlots = configuration.ModuleTypes.Keys.ToArray();
                foreach (var slotIndex in moduleSlots)
                    await ship.ShipModules.SetEquipmentAsync(slotIndex, configuration.ModuleTypes[slotIndex]);
                // foreach (var (slotIndex, weaponType) in configuration.WeaponTypes)
                //     await ship.WeaponBattery.SetEquipmentAsync(slotIndex, weaponType);
                // foreach (var (slotIndex, moduleType) in configuration.ModuleTypes)
                //     await ship.ShipModules.SetEquipmentAsync(slotIndex, moduleType);

                ship.Health.OnShieldChanged += shipView.Shield.UpdatePower;
                ship.OnDied += DestroyShipView;
                ship.WeaponBattery.OnShoot += _soundService.PlayShoot;
                shipView.Shield.UpdatePower(ship.Health.CurrentShield, ship.Health.MaxShield);
                _damageableIdentifier.AddShip(ship, shipView);
                _shipViews.Add(opponentId, shipView);
            }
        }

        private void DestroyShipView(IShip ship)
        {
            var opponentId = _shipConfigurator.Ships.FirstOrDefault(data => data.Value == ship).Key;
            if (!_shipViews.TryGetValue(opponentId, out var shipView))
                return;

            ship.OnDied -= DestroyShipView;
            ship.Health.OnShieldChanged -= shipView.Shield.UpdatePower;
            Object.Destroy(shipView.gameObject);
            _shipViews.Remove(opponentId);
        }
    }
}