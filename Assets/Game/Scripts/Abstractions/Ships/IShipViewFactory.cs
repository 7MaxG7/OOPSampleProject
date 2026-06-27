using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Ships
{
    public interface IShipViewFactory
    {
        UniTask<ShipView> CreateShipViewAsync(IShip ship, Vector3 position, Quaternion rotation);
    }
}