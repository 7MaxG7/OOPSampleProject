using Cysharp.Threading.Tasks;
using Equipment;
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
        private readonly IEquipmentViewFactory _equipmentViewFactory;

        private Transform _shipsParent;

        [Inject]
        public ShipViewFactory(IAssetsInstantiator instantiator, IStaticDataService staticDataService,
            IEquipmentViewFactory equipmentViewFactory)
        {
            _instantiator = instantiator;
            _staticDataService = staticDataService;
            _equipmentViewFactory = equipmentViewFactory;
        }

        public async UniTask<ShipView> CreateShipViewAsync(IShip ship, Vector3 position, Quaternion rotation)
        {
            var config = _staticDataService.GetShip(ship.ShipType);
            var shipView = await _instantiator.CreateAsync<ShipView>(config.Prefab, position, rotation, GetShipsContent());
            shipView.Init(ship.WeaponBattery.MaxEquipmentsAmount, ship.ModuleBattery.MaxEquipmentsAmount, _equipmentViewFactory);
            return shipView;
        }

        private Transform GetShipsContent()
        {
            if (_shipsParent == null)
                _shipsParent = new GameObject(Constants.SHIPS_PARENT_NAME).transform;
            return _shipsParent;
        }
    }
}