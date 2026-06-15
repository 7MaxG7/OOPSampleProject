using System.Collections.Generic;
using Equipment.Data;
using Infrastructure;
using UI.Ship;
using UnityEngine;
using Zenject;

namespace Ships
{
    public sealed class ShipConfigurator : IShipConfigurator
    {
        private readonly IStaticDataService _staticDataService;
        private readonly RulesConfig _rulesConfig;

        public Dictionary<OpponentId, ShipConfiguration> ShipConfigurations { get; } = new();
        public Dictionary<OpponentId, ShipModel> ShipModels { get; } = new();

        [Inject]
        public ShipConfigurator(IStaticDataService staticDataService, RulesConfig rulesConfig)
        {
            _staticDataService = staticDataService;
            _rulesConfig = rulesConfig;
        }

        public void Init()
        {
            foreach (var opponent in _rulesConfig.Opponents)
            {
                var shipData = _staticDataService.GetShip(opponent.ShipType);
                ShipConfigurations.Add(opponent.OpponentId, new ShipConfiguration(shipData));
                ShipModels.Add(opponent.OpponentId, new ShipModel(shipData));
            }
        }

        public void SetWeapon(OpponentId opponentId, int slotIndex, WeaponType weaponType)
        {
            if (!ShipConfigurations.TryGetValue(opponentId, out var configuration)
                || !ShipModels.TryGetValue(opponentId, out var shipModel))
            {
                Debug.LogError($"{this}: No ship configuration for opponent {opponentId}");
                return;
            }

            if (slotIndex >= configuration.WeaponSlotsAmount)
                return;

            configuration.WeaponTypes[slotIndex] = weaponType;
            shipModel.SetWeapon(slotIndex, weaponType);
        }

        public void SetModule(OpponentId opponentId, int slotIndex, ModuleType moduleType)
        {
            if (!ShipConfigurations.TryGetValue(opponentId, out var configuration)
                || !ShipModels.TryGetValue(opponentId, out var shipModel))
            {
                Debug.LogError($"{this}: No ship configuration for opponent {opponentId}");
                return;
            }

            if (slotIndex >= configuration.ModuleSlotsAmount)
                return;

            configuration.ModuleTypes[slotIndex] = moduleType;
            shipModel.SetModule(slotIndex, moduleType);
        }
    }
}
