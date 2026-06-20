using System.Collections.Generic;
using Equipment;
using Infrastructure;

namespace Ships
{
    public interface IShipConfigurator : ISceneCleanable
    {
        IReadOnlyDictionary<OpponentId, ShipConfiguration> ShipConfigurations { get; }
        IReadOnlyDictionary<OpponentId, IShip> Ships { get; }

        void Init();
        void RegisterShip(OpponentId opponentId, IShip ship);
        void SetWeapon(OpponentId opponentId, int slotIndex, WeaponType weaponType);
        void SetModule(OpponentId opponentId, int slotIndex, ModuleType moduleType);
        bool TryGetShip(OpponentId opponentId, out IShip ship);
    }
}
