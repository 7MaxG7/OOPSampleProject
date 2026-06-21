using Cysharp.Threading.Tasks;
using Infrastructure;
using UnityEngine;
using Utils;
using Zenject;

namespace Ships
{
    public sealed class ShipViewFactory : IShipViewFactory
    {
        private readonly IAssetsInstantiator _instantiator;
        private readonly IStaticDataService _staticDataService;

        private Transform _shipsParent;

        [Inject]
        public ShipViewFactory(IAssetsInstantiator instantiator, IStaticDataService staticDataService)
        {
            _instantiator = instantiator;
            _staticDataService = staticDataService;
        }

        public async UniTask<ShipView> CreateShipViewAsync(ShipType shipType, Vector3 position, Quaternion rotation)
        {
            var config = _staticDataService.GetShip(shipType);
            return await _instantiator.CreateAsync<ShipView>(config.Prefab, position, rotation, GetShipsContent());
        }

        private Transform GetShipsContent()
        {
            if (_shipsParent == null)
                _shipsParent = new GameObject(Constants.SHIPS_PARENT_NAME).transform;
            return _shipsParent;
        }
    }
}