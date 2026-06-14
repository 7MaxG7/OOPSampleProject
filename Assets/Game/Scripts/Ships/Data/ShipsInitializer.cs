using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Infrastructure.ControllersHolder;
using Ships.Views;
using Sounds;
using UI.Ship.Models;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace Ships.Data
{
    public sealed class ShipsInitializer : IShipsInitializer
    {
        public Dictionary<OpponentId, IShip> Ships { get; } = new();

        private readonly IShipsFactory _shipsFactory;
        private readonly ISoundService _soundService;
        private readonly IShipConfigurationsHolder _configurationsHolder;
        private Dictionary<OpponentId, ShipModel> _shipModels;

        [Inject]
        public ShipsInitializer(IShipsFactory shipsFactory, IShipConfigurationsHolder configurationsHolder, ISoundService soundService,
            ICleaner cleaner)
        {
            _shipsFactory = shipsFactory;
            _soundService = soundService;
            _configurationsHolder = configurationsHolder;
            cleaner.AddCleanable(this);
        }

        public void CleanUp()
            => CleanUpShips(ship => ship.CleanUp());

        public void SceneCleanUp()
            => CleanUpShips(ship => ship.SceneCleanUp());

        public async UniTask CreateShipsAsync()
        {
            _shipModels = _configurationsHolder.ShipModels;
            foreach (var opponentId in _shipModels.Keys)
            {
                var location = GetOpponentLocation(opponentId);
                if (Ships.ContainsKey(opponentId))
                    continue;

                var ship = await _shipsFactory.CreateShipAsync(_shipModels[opponentId], location.Position, location.Rotation);
                ship.WeaponBattery.OnShoot += _soundService.PlayShoot;
                _shipModels[opponentId].OnWeaponChange += ship.WeaponBattery.SetEquipment;
                _shipModels[opponentId].OnModuleChange += ship.ShipModules.SetEquipment;
                Ships.Add(opponentId, ship);
            }
        }

        private void CleanUpShips(Action<IShip> cleanUpShip)
        {
            foreach (var (opponentId, ship) in Ships)
            {
                ship.WeaponBattery.OnShoot -= _soundService.PlayShoot;
                _shipModels[opponentId].OnWeaponChange -= ship.WeaponBattery.SetEquipment;
                _shipModels[opponentId].OnModuleChange -= ship.ShipModules.SetEquipment;
                cleanUpShip.Invoke(ship);
            }

            Ships.Clear();
        }

        private (Vector3 Position, Quaternion Rotation) GetOpponentLocation(OpponentId opponentId)
        {
            var spawnMarker = Object.FindObjectsOfType<ShipSpawnerMarker>()
                .FirstOrDefault(data => data.OpponentId == opponentId);
            return spawnMarker != null ? (spawnMarker.transform.position, spawnMarker.transform.rotation) : default;
        }
    }
}