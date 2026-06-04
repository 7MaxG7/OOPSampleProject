using Cysharp.Threading.Tasks;
using Ships;
using UnityEngine;

namespace Services
{
    public interface IShipsFactory
    {
        void PrepareRoot();
        UniTask<IShip> CreateShipAsync(ShipModel shipModel, Vector3 position, Quaternion rotation);
    }
}