using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Infrastructure;
using Ships;
using Ui;
using UnityEngine;

namespace UI.ShipSetup
{
    public abstract class BaseEquipmentSelectController
    {
        protected readonly IShipConfigurator ShipConfigurator;
        
        protected OpponentId OpponentId;
        protected int SlotIndex;
        
        private readonly ICancellationTokenProvider _tokenProvider;
        private readonly IUiFactory _uiFactory;
        private readonly UiConfig _uiConfig;

        private EquipmentSelectView _equipmentSelectView;

        private readonly List<SlotUiView> _equipmentsSlots = new();

        protected BaseEquipmentSelectController(IShipConfigurator shipConfigurator, ICancellationTokenProvider tokenProvider,
            IUiFactory uiFactory, UiConfig uiConfig)
        {
            ShipConfigurator = shipConfigurator;
            _tokenProvider = tokenProvider;
            _uiFactory = uiFactory;
            _uiConfig = uiConfig;
        }

        protected abstract UniTask SetupEquipSelectPanelAsync();

        public async UniTask InitAsync(EquipmentSelectView view)
        {
            _equipmentSelectView = view;
            _equipmentSelectView.Init(_uiConfig.FadeAnimDuration);
            await SetupEquipSelectPanelAsync();
        }

        public void Clean()
        {
            foreach (var slot in _equipmentsSlots)
                slot.SelectButton.onClick.RemoveAllListeners();
            _equipmentsSlots.Clear();
        }

        public async UniTaskVoid ShowAsync(OpponentId opponentId, int slotIndex, Vector3 position)
        {
            if (opponentId == OpponentId && slotIndex == SlotIndex && _equipmentSelectView.IsVisible())
                return;

            using var cts = _tokenProvider.CreateLocalCts();

            OpponentId = opponentId;
            SlotIndex = slotIndex;

            await _equipmentSelectView.SetVisibleAsync(false, cts.Token, 0.3f);
            _equipmentSelectView.Locate(opponentId, position);
            await _equipmentSelectView.SetVisibleAsync(true, cts.Token);
        }

        public async UniTaskVoid HideAsync()
        {
            using var cts = _tokenProvider.CreateLocalCts();
            await _equipmentSelectView.SetVisibleAsync(false, cts.Token);
        }

        protected async UniTask<SlotUiView> CreateEquipmentSelectSlotAsync()
        {
            var selectUiSlot = await _uiFactory.CreateSelectEquipmentSlotAsync(_equipmentSelectView.EquipmentsContent);
            _equipmentsSlots.Add(selectUiSlot);
            return selectUiSlot;
        }
    }
}