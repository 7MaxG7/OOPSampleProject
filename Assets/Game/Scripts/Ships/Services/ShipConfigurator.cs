using System.Collections.Generic;
using Equipment;
using Infrastructure;
using UnityEngine;
using Zenject;

namespace Ships
{
    public sealed class ShipConfigurator : IShipConfigurator
    {
        public IReadOnlyDictionary<OpponentId, IShip> Ships => _ships;
        public IReadOnlyDictionary<OpponentId, ShipConfiguration> ShipConfigurations => _shipConfigurations;

        private readonly IStaticDataService _staticDataService;
        private readonly RulesConfig _rulesConfig;
        
        private readonly Dictionary<OpponentId, IShip> _ships = new();
        private readonly Dictionary<OpponentId, ShipConfiguration> _shipConfigurations = new();

        [Inject]
        public ShipConfigurator(IStaticDataService staticDataService, RulesConfig rulesConfig, ICleaner cleaner)
        {
            _staticDataService = staticDataService;
            _rulesConfig = rulesConfig;
            cleaner.AddCleanable(this);
        }

        public void Init()
        {
            foreach (var opponent in _rulesConfig.Opponents)
            {
                var shipData = _staticDataService.GetShip(opponent.ShipType);
                _shipConfigurations.Add(opponent.OpponentId, new ShipConfiguration(shipData));
            }
        }

        public void CleanUp()
        {
            foreach (var (opponentId, ship) in _ships)
            {
                if (!TryGetConfiguration(opponentId, out var configuration))
                    continue;

                ship.WeaponBattery.OnEquipmentChanged -= configuration.SetWeapon;
                ship.ModuleBattery.OnEquipmentChanged -= configuration.SetModule;
            }
            
            // TODO. Clear it after all other clearings (including UI)
            // _ships.Clear();
        }

        public void RegisterShip(OpponentId opponentId, IShip ship)
        {
            _ships[opponentId] = ship;
            if (!TryGetConfiguration(opponentId, out var configuration))
                return;

            ship.WeaponBattery.OnEquipmentChanged += configuration.SetWeapon;
            ship.ModuleBattery.OnEquipmentChanged += configuration.SetModule;
        }

        public void SetWeapon(OpponentId opponentId, int slotIndex, WeaponType weaponType)
        {
            if (TryGetShip(opponentId, out var ship))
                ship.WeaponBattery.SetEquipment(slotIndex, weaponType);
        }

        public void SetModule(OpponentId opponentId, int slotIndex, ModuleType moduleType)
        {
            if (TryGetShip(opponentId, out var ship))
                ship.ModuleBattery.SetEquipment(slotIndex, moduleType);
        }

        public bool TryGetShip(OpponentId opponentId, out IShip ship)
        {
            if (_ships.TryGetValue(opponentId, out ship))
                return true;
            
            Debug.LogError($"{this}: No ship for opponent {opponentId}");
            return false;
        }

        private bool TryGetConfiguration(OpponentId opponentId, out ShipConfiguration configuration)
        {
            if (_shipConfigurations.TryGetValue(opponentId, out configuration))
                return true;
            
            Debug.LogError($"{this}: No ship configuration for opponent {opponentId}");
            return false;
        }
    }
}