using System.Collections.Generic;
using Equipment.Data;
using UI.Ship;

namespace Ships
{
    public interface IShipConfigurator
    {
        Dictionary<OpponentId, ShipConfiguration> ShipConfigurations { get; }
        Dictionary<OpponentId, ShipModel> ShipModels { get; }

        void Init();
        void SetWeapon(OpponentId opponentId, int slotIndex, WeaponType weaponType);
        void SetModule(OpponentId opponentId, int slotIndex, ModuleType moduleType);
    }
}
