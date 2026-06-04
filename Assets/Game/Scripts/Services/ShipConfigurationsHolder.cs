using System.Collections.Generic;
using Configs.Data;
using Enums;
using Ships;
using Zenject;

namespace Services
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
                var shipData = _staticDataService.GetShipData(opponent.ShipType);
                ShipModels.Add(opponent.OpponentId, new ShipModel(shipData));
            }
        }
    }
}