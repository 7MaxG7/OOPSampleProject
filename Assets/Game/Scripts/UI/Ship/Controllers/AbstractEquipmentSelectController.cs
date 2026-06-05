using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
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
    public class AbstractEquipmentSelectController<TType> : ICleanable where TType : Enum
    {
        protected readonly AbstractEquipmentSelectView<TType> EquipmentSelectView;
        protected readonly Dictionary<OpponentId, ShipModel> ShipModels;
        private readonly ICancellationTokenProvider _tokenProvider;

        protected OpponentId OpponentId;
        protected int SlotIndex;
        
        private IUiFactory _uiFactory;
        private float _fadeAnimDuration;

        protected AbstractEquipmentSelectController(AbstractEquipmentSelectView<TType> equipmentSelectView
            , Dictionary<OpponentId,ShipModel> shipModels, ICancellationTokenProvider tokenProvider)
        {
            EquipmentSelectView = equipmentSelectView;
            ShipModels = shipModels;
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