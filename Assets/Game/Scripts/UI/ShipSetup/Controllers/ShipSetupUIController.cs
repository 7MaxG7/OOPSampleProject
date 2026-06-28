using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Infrastructure;
using Ships;
using Ui;
using UnityEngine;

namespace UI.ShipSetup
{
    public sealed class ShipSetupUIController
    {
        private readonly IStaticDataService _staticDataService;
        private readonly IUIFactory _uiFactory;
        private readonly UIConfig _uiConfig;
        private readonly ICancellationTokenProvider _tokenProvider;
        private readonly IShipConfigurator _shipConfigurator;
        private readonly IShipSetupUIService _shipSetupUIService;
        private ShipSetupUIModel _model;

        private WeaponSelectPanelUIController _weaponSelectPanelController;
        private ModuleSelectPanelUIController _moduleSelectPanelController;
        private ShipSetupUIView _view;

        private readonly Dictionary<OpponentId, ShipSetupPanelUIController> _shipPanelControllers = new();

        public ShipSetupUIController(IShipConfigurator shipConfigurator, ICancellationTokenProvider tokenProvider,
            IStaticDataService staticDataService, IUIFactory uiFactory, UIConfig uiConfig, IShipSetupUIService shipSetupUIService)
        {
            _shipConfigurator = shipConfigurator;
            _tokenProvider = tokenProvider;
            _staticDataService = staticDataService;
            _uiFactory = uiFactory;
            _uiConfig = uiConfig;
            _shipSetupUIService = shipSetupUIService;
        }

        public async UniTask InitAsync(ShipSetupUIModel model, ShipSetupUIView view, Action switchState)
        {
            _model = model;
            _view = view;
            
            await CreateWeaponSelectPanelAsync();
            await CreateModuleSelectPanelAsync();

            foreach (var (opponentId, shipPanelModel) in _model.ShipSetupPanels)
                await InitShipPanelAsync(opponentId, shipPanelModel);

            _view.HideAllButton.onClick.AddListener(HideSelectPanels);
            _view.SetupCompleteButton.onClick.AddListener(() => switchState?.Invoke());
        }

        public void Clean()
        {
            foreach (var panel in _shipPanelControllers.Values)
                panel.Clean();

            _shipPanelControllers.Clear();

            _weaponSelectPanelController.Clean();
            _moduleSelectPanelController.Clean();

            _view.SetupCompleteButton.onClick.RemoveAllListeners();
            _view.HideAllButton.onClick.RemoveAllListeners();
        }

        private async UniTask CreateWeaponSelectPanelAsync()
        {
            _weaponSelectPanelController = new WeaponSelectPanelUIController(_shipConfigurator, _uiFactory, _shipSetupUIService, _tokenProvider,
                _staticDataService, _uiConfig);
            await _weaponSelectPanelController.InitAsync(_view.WeaponSelectPanel);
        }

        private async UniTask CreateModuleSelectPanelAsync()
        {
            _moduleSelectPanelController = new ModuleSelectPanelUIController(_shipConfigurator, _uiFactory, _shipSetupUIService, _tokenProvider,
                _staticDataService, _uiConfig);
            await _moduleSelectPanelController.InitAsync(_view.ModuleSelectPanel);
        }

        private async UniTask InitShipPanelAsync(OpponentId opponentId, ShipSetupPanelUIModel shipPanelModel)
        {
            var shipPanelView = _view.ShipPanels.FirstOrDefault(view => view.OpponentId == opponentId);
            if (shipPanelView == null)
            {
                Debug.LogError($"{this}: No ship setup panel for opponent {opponentId}");
                return;
            }
            
            var controller = new ShipSetupPanelUIController(_tokenProvider, _shipSetupUIService, _uiFactory);
            await controller.InitAsync(shipPanelModel, shipPanelView, _weaponSelectPanelController, _moduleSelectPanelController);
            _shipPanelControllers.Add(opponentId, controller);
        }

        private void HideSelectPanels()
        {
            _weaponSelectPanelController.HideAsync().Forget();
            _moduleSelectPanelController.HideAsync().Forget();
        }
    }
}