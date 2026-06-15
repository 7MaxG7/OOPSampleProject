using System;
using Cysharp.Threading.Tasks;
using Infrastructure;
using Ships;
using UnityEngine;

namespace UI.Ship
{
    public class BaseEquipmentSelectController<TType> : ICleanable where TType : Enum
    {
        protected readonly BaseEquipmentSelectView<TType> EquipmentSelectView;
        protected readonly IShipConfigurator ShipConfigurator;
        private readonly ICancellationTokenProvider _tokenProvider;

        protected OpponentId OpponentId;
        protected int SlotIndex;

        protected BaseEquipmentSelectController(BaseEquipmentSelectView<TType> equipmentSelectView,
            IShipConfigurator shipConfigurator, ICancellationTokenProvider tokenProvider)
        {
            EquipmentSelectView = equipmentSelectView;
            ShipConfigurator = shipConfigurator;
            _tokenProvider = tokenProvider;
        }

        public void CleanUp()
        {
            EquipmentSelectView.CleanUp();
        }

        public async UniTaskVoid ShowAsync(OpponentId opponentId, int slotIndex, Vector3 position)
        {
            if (opponentId == OpponentId && slotIndex == SlotIndex && EquipmentSelectView.IsVisible())
                return;

            using var cts = _tokenProvider.CreateLocalCts();

            OpponentId = opponentId;
            SlotIndex = slotIndex;

            await EquipmentSelectView.SetVisibleAsync(false, cts.Token, 0.3f);
            EquipmentSelectView.Locate(opponentId, position);
            await EquipmentSelectView.SetVisibleAsync(true, cts.Token);
        }

        public async UniTaskVoid HideAsync()
        {
            using var cts = _tokenProvider.CreateLocalCts();
            await EquipmentSelectView.SetVisibleAsync(false, cts.Token);
        }
    }
}