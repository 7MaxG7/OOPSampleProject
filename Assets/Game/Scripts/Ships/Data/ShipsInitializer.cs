using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Infrastructure;
using Sounds;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace Ships
{
    public sealed class ShipsInitializer : IShipsInitializer
    {
        public Dictionary<OpponentId, IShip> Ships { get; } = new();

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
            => CleanUpShips(ship => ship.CleanUp());

        public void SceneCleanUp()
            => CleanUpShips(ship => ship.SceneCleanUp());

        public async UniTask CreateShipsAsync()
        {
            foreach (var opponentId in _shipConfigurator.ShipConfigurations.Keys)
            {
                var location = GetOpponentLocation(opponentId);
                if (Ships.ContainsKey(opponentId))
                    continue;

                var ship = await _shipsFactory.CreateShipAsync(_shipConfigurator.ShipConfigurations[opponentId], location.Position,
                    location.Rotation);
                ship.WeaponBattery.OnShoot += _soundService.PlayShoot;
                _shipConfigurator.ShipModels[opponentId].OnWeaponChange += ship.WeaponBattery.SetEquipment;
                _shipConfigurator.ShipModels[opponentId].OnModuleChange += ship.ShipModules.SetEquipment;
                Ships.Add(opponentId, ship);
            }
        }

        private void CleanUpShips(Action<IShip> cleanUpShip)
        {
            foreach (var (opponentId, ship) in Ships)
            {
                ship.WeaponBattery.OnShoot -= _soundService.PlayShoot;
                _shipConfigurator.ShipModels[opponentId].OnWeaponChange -= ship.WeaponBattery.SetEquipment;
                _shipConfigurator.ShipModels[opponentId].OnModuleChange -= ship.ShipModules.SetEquipment;
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
