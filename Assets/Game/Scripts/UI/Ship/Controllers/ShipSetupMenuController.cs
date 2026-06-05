using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Equipment.Data;
using Infrastructure;
using Infrastructure.ControllersHolder;
using Ships;
using Ships.Data;
using Ui;
using UI.Ship.Models;
using UI.Ship.Views;
using UnityEngine;

namespace UI.Ship
{
    public sealed class ShipSetupMenuController : ISceneCleanable
    {
        public event Action OnSetupComplete;

        private IStaticDataService _staticDataService;
        private readonly IShipConfigurationsHolder _configurationsHolder;
        private IUiFactory _uiFactory;
        private UiConfig _uiConfig;
        private readonly ICancellationTokenProvider _tokenProvider;

        private WeaponSelectPanelController _weaponSelectPanel;
        private ModuleSelectPanelController _moduleSelectPanel;
        private readonly ShipSetupMenuView _shipSetupMenuView;
        private readonly Dictionary<OpponentId, ShipModel> _shipModels;
        private readonly Dictionary<OpponentId, ShipPanelController> _shipPanels = new();

        public ShipSetupMenuController(ShipSetupMenuView view, Dictionary<OpponentId, ShipModel> shipModels,
            ICancellationTokenProvider tokenProvider)
        {
            _shipSetupMenuView = view;
            _shipModels = shipModels;
            _tokenProvider = tokenProvider;
        }

        public void CleanUp()
            => SceneCleanUp();

        public void SceneCleanUp()
        {
            foreach (var panel in _shipPanels.Values)
            {
                panel.OnWeaponSelectClick -= ShowSelectWeaponPanel;
                panel.OnModuleSelectClick -= ShowSelectModulePanel;
                panel.CleanUp();
            }

            _shipPanels.Clear();

            _weaponSelectPanel.CleanUp();
            _moduleSelectPanel.CleanUp();

            _shipSetupMenuView.SetupCompleteButton.onClick.RemoveAllListeners();
            _shipSetupMenuView.HideAllButton.onClick.RemoveAllListeners();
        }

        public void Init(IStaticDataService staticDataService, IUiFactory uiFactory, UiConfig uiConfig)
        {
            _staticDataService = staticDataService;
            _uiFactory = uiFactory;
            _uiConfig = uiConfig;
        }

        public async UniTask SetupUiAsync(IEnumerable<OpponentId> opponentIds)
        {
            await SetupWeaponSelectPanelAsync();
            await SetupModuleSelectPanelAsync();

            foreach (var opponentId in opponentIds)
                await InitShipPanelAsync(opponentId);

            _shipSetupMenuView.SetupCompleteButton.onClick.AddListener(() => OnSetupComplete?.Invoke());
            _shipSetupMenuView.HideAllButton.onClick.AddListener(HideSelectPanels);
        }

        private async UniTask SetupModuleSelectPanelAsync()
        {
            _moduleSelectPanel = new ModuleSelectPanelController(_shipSetupMenuView.ModuleSelectPanel, _shipModels, _tokenProvider);
            _shipSetupMenuView.ModuleSelectPanel.Init(_uiFactory, _uiConfig.FadeAnimDuration);
            await _moduleSelectPanel.SetupModuledSelectPanelAsync(_staticDataService.GetAllEnabledModules());
        }

        private async UniTask SetupWeaponSelectPanelAsync()
        {
            _weaponSelectPanel = new WeaponSelectPanelController(_shipSetupMenuView.WeaponSelectPanel, _shipModels, _tokenProvider);
            _shipSetupMenuView.WeaponSelectPanel.Init(_uiFactory, _uiConfig.FadeAnimDuration);
            await _weaponSelectPanel.SetupWeaponSelectPanelAsync(_staticDataService.GetAllEnabledWeapons());
        }

        private async UniTask InitShipPanelAsync(OpponentId opponentId)
        {
            var shipPanelView = _shipSetupMenuView.ShipPanels.FirstOrDefault(view => view.OpponentId == opponentId);
            if (shipPanelView == null)
            {
                Debug.LogError($"{this}: No ship setup panel for opponent {opponentId}");
                return;
            }

            var weaponIcons = _staticDataService.GetAllEnabledWeapons()
                .ToDictionary(data => data.WeaponType, data => data.Icon);
            var moduleIcons = _staticDataService.GetAllEnabledModules()
                .ToDictionary(data => data.ModuleType, data => data.Icon);

            var shipPanel = new ShipPanelController(shipPanelView, _uiFactory, weaponIcons, moduleIcons);
            var shipData = _staticDataService.GetShip(_shipModels[opponentId].ShipType);
            await shipPanel.InitAsync(_shipModels[opponentId], shipData.WeaponSlotsAmount, shipData.ModuleSlotsAmount);
            shipPanel.OnWeaponSelectClick += ShowSelectWeaponPanel;
            shipPanel.OnModuleSelectClick += ShowSelectModulePanel;
            _shipPanels.Add(opponentId, shipPanel);
        }

        private void HideSelectPanels()
        {
            _weaponSelectPanel.HideAsync().Forget();
            _moduleSelectPanel.HideAsync().Forget();
        }

        private void ShowSelectWeaponPanel(OpponentId opponentId, int index)
        {
            _moduleSelectPanel.HideAsync().Forget();
            var anchor = _shipPanels[opponentId].GetEquipmentSelectAnchor(EquipmentType.Weapon, index);
            _weaponSelectPanel.ShowAsync(opponentId, index, anchor.position).Forget();
        }

        private void ShowSelectModulePanel(OpponentId opponentId, int index)
        {
            _weaponSelectPanel.HideAsync().Forget();
            var anchor = _shipPanels[opponentId].GetEquipmentSelectAnchor(EquipmentType.Module, index);
            _moduleSelectPanel.ShowAsync(opponentId, index, anchor.position).Forget();
        }
    }
}