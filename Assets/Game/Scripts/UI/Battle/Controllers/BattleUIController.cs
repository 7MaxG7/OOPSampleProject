using System;
using System.Linq;
using System.Threading;
using Battle;
using Cysharp.Threading.Tasks.Linq;
using Infrastructure;
using Ships;
using UI.Battle.Views;
using UnityEngine;

namespace UI.Battle
{
    public sealed class BattleUIController
    {
        private readonly ICancellationTokenProvider _tokenProvider;
        private readonly IWinnerDefiner _winnerDefiner;

        private BattleUiView _view;
        private CancellationTokenSource _cts;

        public BattleUIController(ICancellationTokenProvider tokenProvider, IWinnerDefiner winnerDefiner)
        {
            _tokenProvider = tokenProvider;
            _winnerDefiner = winnerDefiner;
        }

        public void Clean()
        {
            _view.LeaveButton.onClick.RemoveAllListeners();
            _winnerDefiner.OnWinnerDefined -= _view.ShowWinnerLabel;
            _cts.Cancel();
            _cts.Dispose();
            _cts = null;
        }

        public void Init(BattleUIModel model, BattleUiView view, Action leaveBattle)
        {
            _view = view;

            _cts = _tokenProvider.CreateLocalCts();
            foreach (var (opponentId, shipHealthModel) in model.ShipHealthModels)
                InitHealthPanel(opponentId, shipHealthModel, _cts);

            _view.LeaveButton.onClick.AddListener(() => leaveBattle?.Invoke());
            _winnerDefiner.OnWinnerDefined += _view.ShowWinnerLabel;
            _view.Init();
        }

        private void InitHealthPanel(OpponentId opponentId, BattleShipHealthUIModel shipHealthModel, CancellationTokenSource cts)
        {
            var healthPanel = _view.HealthPanels.FirstOrDefault(item => item.OpponentId == opponentId);
            if (healthPanel == null)
            {
                Debug.LogError($"{this}: No health panel for opponent {opponentId}");
                return;
            }

            shipHealthModel.Hp.Subscribe(healthPanel.SetHp, cts.Token);
            shipHealthModel.Shield.Subscribe(healthPanel.SetShield, cts.Token);
        }
    }
}