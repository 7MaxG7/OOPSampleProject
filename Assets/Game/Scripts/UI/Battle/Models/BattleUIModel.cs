using System.Collections.Generic;
using Ships;

namespace UI.Battle
{
    public sealed class BattleUIModel
    {
        public Dictionary<OpponentId, BattleShipHealthUIModel> ShipHealthModels { get; } = new();
    }
}