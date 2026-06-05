using System.Collections.Generic;
using Ships.Data;
using UI.Ship.Models;

namespace Ships
{
    public interface IShipConfigurationsHolder
    {
        Dictionary<OpponentId, ShipModel> ShipModels { get; }
        
        void Init(Opponent[] opponents);
    }
}