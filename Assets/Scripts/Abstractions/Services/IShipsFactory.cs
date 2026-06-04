using Cysharp.Threading.Tasks;
using Abstractions.Ships;
using Ships;
using UnityEngine;

namespace Abstractions.Services
{
    public interface IShipsFactory
    {
        void PrepareRoot();
        UniTask<IShip> CreateShipAsync(ShipModel shipModel, Vector3 position, Quaternion rotation);
    }
}