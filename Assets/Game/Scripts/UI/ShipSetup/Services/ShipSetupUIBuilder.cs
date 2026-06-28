using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Infrastructure;
using Ships;
using Ui;
using Zenject;

namespace UI.ShipSetup
{
    public class ShipSetupUIBuilder : IShipSetupUIBuilder
    {
        private readonly ShipSetupUIModel _shipSetupUIModel;
        private readonly IUiFactory _uiFactory;
        private readonly IShipConfigurator _shipConfigurator;
        private readonly ICancellationTokenProvider _tokenProvider;
        private readonly IStaticDataService _staticDataService;
        private readonly UiConfig _uiConfig;
        private readonly IShipSetupUIService _shipSetupUIService;

        private ShipSetupUIController _shipSetupUIController;

        private bool _isInited;

        [Inject]
        public ShipSetupUIBuilder(ShipSetupUIModel shipSetupUIModel, IUiFactory uiFactory, IShipConfigurator shipConfigurator,
            ICancellationTokenProvider tokenProvider, UiConfig uiConfig, IStaticDataService staticDataService, ICleaner cleaner,
            IShipSetupUIService shipSetupUIService)
        {
            _shipSetupUIModel = shipSetupUIModel;
            _uiFactory = uiFactory;
            _shipConfigurator = shipConfigurator;
            _tokenProvider = tokenProvider;
            _staticDataService = staticDataService;
            _uiConfig = uiConfig;
            _shipSetupUIService = shipSetupUIService;

            cleaner.AddCleanable(this);
        }

        public void CleanUp()
        {
            if (!_isInited)
                return;

            foreach (var (opponentId, shipModel) in _shipSetupUIModel.ShipSetupPanels)
            {
                if (!_shipConfigurator.TryGetShip(opponentId, out var ship))
                    continue;

                ship.WeaponBattery.OnEquipmentChanged -= shipModel.SetWeapon;
                ship.ModuleBattery.OnEquipmentChanged -= shipModel.SetModule;
            }

            _shipSetupUIController.Clean();
            _shipSetupUIModel.ShipSetupPanels.Clear();
            _isInited = false;
        }

        public async UniTask BuildUIAsync(Action switchState)
        {
            await _uiFactory.CreateRootAsync();
            _shipSetupUIController = new ShipSetupUIController(_shipConfigurator, _tokenProvider, _staticDataService, _uiFactory,
                _uiConfig, _shipSetupUIService);
            var view = await _uiFactory.CreateShipSetupUIAsync();
            var model = SetupModel();
            await _shipSetupUIController.InitAsync(model, view, switchState);

            _isInited = true;
        }

        private ShipSetupUIModel SetupModel()
        {
            foreach (var (opponentId, ship) in _shipConfigurator.Ships)
                _shipSetupUIModel.ShipSetupPanels.Add(opponentId, CreateShipPanelModel(ship));

            return _shipSetupUIModel;
        }

        private ShipSetupPanelUIModel CreateShipPanelModel(IShip ship)
        {
            var model = new ShipSetupPanelUIModel();

            for (var i = 0; i < ship.WeaponBattery.MaxEquipmentsAmount; i++)
            {
                var weapon = ship.WeaponBattery.Equipments.GetValueOrDefault(i);
                model.WeaponSlots.Add(i, new(weapon?.WeaponType ?? default));
            }

            for (var i = 0; i < ship.ModuleBattery.MaxEquipmentsAmount; i++)
            {
                var module = ship.ModuleBattery.Equipments.GetValueOrDefault(i);
                model.ModuleSlots.Add(i, new(module?.ModuleType ?? default));
            }

            ship.WeaponBattery.OnEquipmentChanged += model.SetWeapon;
            ship.ModuleBattery.OnEquipmentChanged += model.SetModule;
            return model;
        }
    }
}