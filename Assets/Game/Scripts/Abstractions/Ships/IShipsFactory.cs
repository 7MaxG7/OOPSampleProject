using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Ships
{
    public interface IShipsFactory
    {
        UniTask<IShip> CreateShipAsync(ShipType shipType, Vector3 position, Quaternion rotation);
    }
}
