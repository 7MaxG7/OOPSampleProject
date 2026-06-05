using Cysharp.Threading.Tasks;
using UI.Ship.Models;
using UnityEngine;

namespace Ships
{
    public interface IShipsFactory
    {
        void PrepareRoot();
        UniTask<IShip> CreateShipAsync(ShipModel shipModel, Vector3 position, Quaternion rotation);
    }
}