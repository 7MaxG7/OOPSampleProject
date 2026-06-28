using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Equipment;
using Infrastructure;
using Ships;
using Ui;
using UnityEngine;

namespace UI.ShipSetup
{
    public sealed class ShipSetupPanelUIController
    {
        private readonly IUIFactory _uiFactory;
        private readonly ICancellationTokenProvider _tokenProvider;
        private readonly IShipSetupUIService _shipSetupUIService;

        private ShipSetupPanelUIModel _model;
        private ShipSetupPanelUIView _view;
        private WeaponSelectPanelUIController _weaponSelectPanelController;
        private ModuleSelectPanelUIController _moduleSelectPanelController;

        private OpponentId OpponentId => _view.OpponentId;
        private CancellationTokenSource _cts;
        private readonly Dictionary<int, ShipSlotUIView> _weaponSlots = new();
        private readonly Dictionary<int, ShipSlotUIView> _moduleSlots = new();
        private bool _isCleaned;

        public ShipSetupPanelUIController(ICancellationTokenProvider tokenProvider, IShipSetupUIService shipSetupUIService,
            IUIFactory uiFactory)
        {
            _tokenProvider = tokenProvider;
            _shipSetupUIService = shipSetupUIService;
            _uiFactory = uiFactory;
        }

        public async UniTask InitAsync(ShipSetupPanelUIModel model, ShipSetupPanelUIView view,
            WeaponSelectPanelUIController weaponSelectPanelController, ModuleSelectPanelUIController moduleSelectPanelController)
        {
            _model = model;
            _view = view;
            _moduleSelectPanelController = moduleSelectPanelController;
            _weaponSelectPanelController = weaponSelectPanelController;

            _cts = _tokenProvider.CreateLocalCts();
            await SetupWeaponsPanelAsync(_cts);
            await SetupModulesPanelAsync(_cts);
        }

        public void Clean()
        {
            if (_isCleaned)
                return;

            foreach (var slot in _weaponSlots.Values)
                slot.SelectButton.onClick.RemoveAllListeners();
            _weaponSlots.Clear();

            foreach (var slot in _moduleSlots.Values)
                slot.SelectButton.onClick.RemoveAllListeners();
            _moduleSlots.Clear();

            _cts.Cancel();
            _cts.Dispose();
            _cts = null;

            _isCleaned = true;
        }

        private async UniTask SetupWeaponsPanelAsync(CancellationTokenSource cts)
        {
            foreach (var index in _model.WeaponSlots.Keys)
            {
                var slotIndex = index;
                var slotView = await _uiFactory.CreateShipEquipmentSlotAsync(_view.WeaponSlotsContent);
                _model.WeaponSlots[slotIndex].Subscribe(weaponType =>
                {
                    var icon = _shipSetupUIService.GetWeaponIcon(weaponType);
                    slotView.SetIcon(icon);
                }, cts.Token);

                slotView.SelectButton.onClick.AddListener(() => ShowSelectWeaponPanel(slotIndex));
                _weaponSlots.Add(slotIndex, slotView);
            }

            void ShowSelectWeaponPanel(int slotIndex)
                => ShowSelectEquipPanel(_moduleSelectPanelController, _weaponSelectPanelController, EquipmentType.Weapon, slotIndex);
        }

        private async UniTask SetupModulesPanelAsync(CancellationTokenSource cts)
        {
            foreach (var index in _model.ModuleSlots.Keys)
            {
                var slotIndex = index;
                var slotView = await _uiFactory.CreateShipEquipmentSlotAsync(_view.ModuleSlotsContent);
                _model.ModuleSlots[slotIndex].Subscribe(moduleType =>
                {
                    var icon = _shipSetupUIService.GetModuleIcon(moduleType);
                    slotView.SetIcon(icon);
                }, cts.Token);

                slotView.SelectButton.onClick.AddListener(() => ShowSelectModulePanel(slotIndex));
                _moduleSlots.Add(slotIndex, slotView);
            }

            void ShowSelectModulePanel(int slotIndex)
                => ShowSelectEquipPanel(_weaponSelectPanelController, _moduleSelectPanelController, EquipmentType.Module, slotIndex);
        }

        private void ShowSelectEquipPanel(BaseEquipmentSelectUIController hidingPanel, BaseEquipmentSelectUIController showingPanel,
            EquipmentType equipmentType, int slotIndex)
        {
            hidingPanel.HideAsync().Forget();
            var anchor = GetEquipmentSelectAnchor(equipmentType, slotIndex);
            showingPanel.ShowAsync(OpponentId, slotIndex, anchor.position).Forget();
        }

        private Transform GetEquipmentSelectAnchor(EquipmentType type, int index)
        {
            var slot = type switch
            {
                EquipmentType.Weapon => _weaponSlots.GetValueOrDefault(index),
                EquipmentType.Module => _moduleSlots.GetValueOrDefault(index),
                _ => null,
            };

            if (slot == null)
            {
                Debug.LogError($"{this}: No slop for type {type.ToString()} index {index}");
                return null;
            }

            var opponentAnchor = slot.SelectPanelAnchor.FirstOrDefault(anchor => anchor.OpponentId == OpponentId);
            if (opponentAnchor == null)
            {
                Debug.LogError($"{this}: Ship slot anchor for opponent {OpponentId} is not found.");
                return null;
            }

            return opponentAnchor.Anchor;
        }
    }
}