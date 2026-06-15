using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Ships
{
    public interface IShipsFactory
    {
        UniTask<IShip> CreateShipAsync(ShipConfiguration configuration, Vector3 position, Quaternion rotation);
    }
}
