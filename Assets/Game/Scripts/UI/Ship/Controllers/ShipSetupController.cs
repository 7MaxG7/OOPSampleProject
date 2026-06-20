using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Infrastructure;
using Ships;
using Ui;
using UnityEngine;

namespace UI.Ship
{
    public sealed class ShipSetupController : ISceneCleanable
    {
        public event Action OnSetupComplete;

        private readonly IStaticDataService _staticDataService;
        private readonly IUiFactory _uiFactory;
        private readonly UiConfig _uiConfig;
        private readonly ICancellationTokenProvider _tokenProvider;
        private readonly IShipConfigurator _shipConfigurator;
        private readonly IShipSetupUIService _shipSetupUIService;

        private WeaponSelectPanelController _weaponSelectPanel;
        private ModuleSelectPanelController _moduleSelectPanel;
        private readonly ShipSetupView _shipSetupView;

        private readonly Dictionary<OpponentId, ShipSetupPanelUIModel> _shipModels = new();
        private readonly Dictionary<OpponentId, ShipSetupPanelUIController> _shipPanelControllers = new();

        public ShipSetupController(ShipSetupView view, IShipConfigurator shipConfigurator, ICancellationTokenProvider tokenProvider,
            IStaticDataService staticDataService, IUiFactory uiFactory, UiConfig uiConfig, IShipSetupUIService shipSetupUIService,
            ICleaner cleaner)
        {
            _shipSetupView = view;
            _shipConfigurator = shipConfigurator;
            _tokenProvider = tokenProvider;
            _staticDataService = staticDataService;
            _uiFactory = uiFactory;
            _uiConfig = uiConfig;
            _shipSetupUIService = shipSetupUIService;
            cleaner.AddCleanable(this);
        }

        public void CleanUp()
        {
            foreach (var (opponentId, shipModel) in _shipModels)
            {
                if (!_shipConfigurator.Ships.TryGetValue(opponentId, out var ship))
                {
                    Debug.LogWarning($"{this}: Cannot get ship of opponent {opponentId}");
                    continue;
                }

                ship.WeaponBattery.OnEquipmentChanged -= shipModel.SetWeapon;
                ship.ShipModules.OnEquipmentChanged -= shipModel.SetModule;
            }

            foreach (var panel in _shipPanelControllers.Values)
                panel.CleanUp();

            _shipModels.Clear();
            _shipPanelControllers.Clear();

            _weaponSelectPanel.CleanUp();
            _moduleSelectPanel.CleanUp();

            _shipSetupView.SetupCompleteButton.onClick.RemoveAllListeners();
            _shipSetupView.HideAllButton.onClick.RemoveAllListeners();
        }

        public async UniTask SetupUiAsync()
        {
            await SetupWeaponSelectPanelAsync();
            await SetupModuleSelectPanelAsync();

            foreach (var opponentId in _shipConfigurator.Ships.Keys)
                await InitShipPanelAsync(opponentId);

            _shipSetupView.SetupCompleteButton.onClick.AddListener(() => OnSetupComplete?.Invoke());
            _shipSetupView.HideAllButton.onClick.AddListener(HideSelectPanels);
        }

        private async UniTask SetupWeaponSelectPanelAsync()
        {
            _weaponSelectPanel = new WeaponSelectPanelController(_shipConfigurator, _uiFactory, _shipSetupUIService, _tokenProvider,
                _staticDataService, _uiConfig);
            await _weaponSelectPanel.InitAsync(_shipSetupView.WeaponSelectPanel);
        }

        private async UniTask SetupModuleSelectPanelAsync()
        {
            _moduleSelectPanel = new ModuleSelectPanelController(_shipConfigurator, _uiFactory, _shipSetupUIService, _tokenProvider,
                _staticDataService, _uiConfig);
            await _moduleSelectPanel.InitAsync(_shipSetupView.ModuleSelectPanel);
        }

        private async UniTask InitShipPanelAsync(OpponentId opponentId)
        {
            var shipPanelView = _shipSetupView.ShipPanels.FirstOrDefault(view => view.OpponentId == opponentId);
            if (shipPanelView == null)
            {
                Debug.LogError($"{this}: No ship setup panel for opponent {opponentId}");
                return;
            }

            if (!_shipConfigurator.TryGetShip(opponentId, out var ship))
                return;

            var model = CreateModel(opponentId, ship);
            var shipPanel = new ShipSetupPanelUIController(_tokenProvider, _shipSetupUIService, _uiFactory);
            await shipPanel.InitAsync(model, shipPanelView, _weaponSelectPanel, _moduleSelectPanel);
            _shipPanelControllers.Add(opponentId, shipPanel);
        }

        private void HideSelectPanels()
        {
            _weaponSelectPanel.HideAsync().Forget();
            _moduleSelectPanel.HideAsync().Forget();
        }

        private ShipSetupPanelUIModel CreateModel(OpponentId opponentId, IShip ship)
        {
            var model = new ShipSetupPanelUIModel();

            for (var i = 0; i < ship.WeaponBattery.MaxEquipmentsAmount; i++)
            {
                var weapon = ship.WeaponBattery.Equipments.GetValueOrDefault(i);
                model.WeaponSlots.Add(i, new(weapon?.WeaponType ?? default));
            }

            for (var i = 0; i < ship.ShipModules.MaxEquipmentsAmount; i++)
            {
                var module = ship.ShipModules.Equipments.GetValueOrDefault(i);
                model.ModuleSlots.Add(i, new(module?.ModuleType ?? default));
            }

            ship.WeaponBattery.OnEquipmentChanged += model.SetWeapon;
            ship.ShipModules.OnEquipmentChanged += model.SetModule;
            _shipModels.Add(opponentId, model);
            return model;
        }
    }
}