using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Abstractions;
using Abstractions.Infrastructure;
using Abstractions.Services;
using Abstractions.Ships;
using Enums;
using Sounds;
using Zenject;

namespace Ships
{
    public sealed class ShipsInitializer : IShipsInitializer
    {
        public Dictionary<OpponentId, IShip> Ships { get; } = new();

        private readonly IShipsFactory _shipsFactory;
        private readonly ILocationFinder _locationFinder;
        private readonly ISoundPlayer _soundPlayer;
        private readonly IShipConfigurationsHolder _configurationsHolder;
        private Dictionary<OpponentId,ShipModel> _shipModels;


        [Inject]
        public ShipsInitializer(IShipsFactory shipsFactory, IShipConfigurationsHolder configurationsHolder
            , ILocationFinder locationFinder, ISoundPlayer soundPlayer, ICleaner cleaner)
        {
            _shipsFactory = shipsFactory;
            _locationFinder = locationFinder;
            _soundPlayer = soundPlayer;
            _configurationsHolder = configurationsHolder;
            cleaner.AddCleanable(this);
        }

        public void CleanUp()
        {
            foreach (var (opponentId, ship) in Ships)
            {
                ship.WeaponBattery.OnShoot -= _soundPlayer.PlayShoot;
                _shipModels[opponentId].OnWeaponChange -= ship.WeaponBattery.SetEquipmentSync;
                _shipModels[opponentId].OnModuleChange -= ship.ShipModules.SetEquipmentSync;
                ship.CleanUp();
            }
            Ships.Clear();
        }

        public void SceneCleanUp()
        {
            foreach (var (opponentId, ship) in Ships)
            {
                ship.WeaponBattery.OnShoot -= _soundPlayer.PlayShoot;
                _shipModels[opponentId].OnWeaponChange -= ship.WeaponBattery.SetEquipmentSync;
                _shipModels[opponentId].OnModuleChange -= ship.ShipModules.SetEquipmentSync;
                ship.SceneCleanUp();
            }
            Ships.Clear();
        }

        public async UniTask PrepareShipsAsync()
        {
            _shipsFactory.PrepareRoot();
            _shipModels = _configurationsHolder.ShipModels;
            foreach (var opponentId in _shipModels.Keys)
            {
                var position = _locationFinder.GetOpponentLocation(opponentId, out var rotation);
                if (!position.HasValue || Ships.ContainsKey(opponentId))
                    continue;

                var ship = await _shipsFactory.CreateShipAsync(_shipModels[opponentId], position.Value, rotation);
                ship.WeaponBattery.OnShoot += _soundPlayer.PlayShoot;
                _shipModels[opponentId].OnWeaponChange += ship.WeaponBattery.SetEquipmentSync;
                _shipModels[opponentId].OnModuleChange += ship.ShipModules.SetEquipmentSync;
                Ships.Add(opponentId, ship);
            }
        }
    }
}