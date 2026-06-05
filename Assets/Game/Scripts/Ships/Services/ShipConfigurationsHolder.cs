using System.Collections.Generic;
using Infrastructure;
using Ships.Data;
using UI.Ship.Models;
using Zenject;

namespace Ships
{
    public sealed class ShipConfigurationsHolder : IShipConfigurationsHolder
    {
        private readonly IStaticDataService _staticDataService;
        public Dictionary<OpponentId, ShipModel> ShipModels { get; } = new();


        [Inject]
        public ShipConfigurationsHolder(IStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
        }

        public void Init(Opponent[] opponents)
        {
            foreach (var opponent in opponents)
            {
                var shipData = _staticDataService.GetShip(opponent.ShipType);
                ShipModels.Add(opponent.OpponentId, new ShipModel(shipData));
            }
        }
    }
}