using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Ships
{
    public interface IShipViewFactory
    {
        UniTask<ShipView> CreateShipViewAsync(ShipType shipType, Vector3 position, Quaternion rotation);
    }
}