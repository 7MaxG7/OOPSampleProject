using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Infrastructure;
using Ships;
using Ui;
using UnityEngine;

namespace UI.ShipSetup
{
    public abstract class BaseEquipmentSelectUIController
    {
        protected readonly IShipConfigurator ShipConfigurator;
        
        protected OpponentId OpponentId;
        protected int SlotIndex;
        
        private readonly ICancellationTokenProvider _tokenProvider;
        private readonly IUIFactory _uiFactory;
        private readonly UIConfig _uiConfig;

        private EquipmentSelectUIView _view;

        private readonly List<SlotUIView> _equipmentsSlots = new();

        protected BaseEquipmentSelectUIController(IShipConfigurator shipConfigurator, ICancellationTokenProvider tokenProvider,
            IUIFactory uiFactory, UIConfig uiConfig)
        {
            ShipConfigurator = shipConfigurator;
            _tokenProvider = tokenProvider;
            _uiFactory = uiFactory;
            _uiConfig = uiConfig;
        }

        protected abstract UniTask SetupEquipSelectPanelAsync();

        public async UniTask InitAsync(EquipmentSelectUIView view)
        {
            _view = view;
            _view.Init(_uiConfig.FadeAnimDuration);
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
            if (opponentId == OpponentId && slotIndex == SlotIndex && _view.IsVisible())
                return;

            using var cts = _tokenProvider.CreateLocalCts();

            OpponentId = opponentId;
            SlotIndex = slotIndex;

            await _view.SetVisibleAsync(false, cts.Token, 0.3f);
            _view.Locate(opponentId, position);
            await _view.SetVisibleAsync(true, cts.Token);
        }

        public async UniTaskVoid HideAsync()
        {
            using var cts = _tokenProvider.CreateLocalCts();
            await _view.SetVisibleAsync(false, cts.Token);
        }

        protected async UniTask<SlotUIView> CreateEquipmentSelectSlotAsync()
        {
            var selectUiSlot = await _uiFactory.CreateSelectEquipmentSlotAsync(_view.EquipmentsContent);
            _equipmentsSlots.Add(selectUiSlot);
            return selectUiSlot;
        }
    }
}