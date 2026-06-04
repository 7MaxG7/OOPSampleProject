using System;
using System.Collections.Generic;
using Enums;
using Infrastructure;
using Ships;
using UI.Battle.Views;
using UnityEngine;

namespace UI.Battle
{
    public sealed class BattleUiController : ICleanable
    {
        public event Action OnBattleLeft;
        
        private readonly BattleUiView _uiView;
        private Dictionary<OpponentId,IShip> _ships = new();


        public BattleUiController(BattleUiView uiView)
        {
            _uiView = uiView;
        }

        public void CleanUp()
        {
            _uiView.LeaveButton.onClick.RemoveAllListeners();
            foreach (var (opponent, ship) in _ships)
            {
                var healthPanel = _uiView.GetHealthPanel(opponent);
                ship.Health.OnHpChanged -= healthPanel.SetCurrentHp;
                ship.Health.OnShieldChanged -= healthPanel.SetCurrentShield;
            }
        }

        public void SetupUi(Dictionary<OpponentId, IShip> ships)
        {
            _ships = ships;
            foreach (var opponentId in _ships.Keys) 
                InitHealthPanel(opponentId);
            
            _uiView.LeaveButton.onClick.AddListener(() => OnBattleLeft?.Invoke());
            _uiView.HideBattleEndObjects();
        }

        public void ShowBattleEnd(IShip winner) 
            => _uiView.ShowWinnerLable(winner);

        private void InitHealthPanel(OpponentId opponentId)
        {
            var healthPanel = _uiView.GetHealthPanel(opponentId);
            if (healthPanel == null)
            {
                Debug.LogError($"{this}: No health panel for opponent {opponentId}");
                return;
            }

            var health = _ships[opponentId].Health;
            health.OnHpChanged += healthPanel.SetCurrentHp;
            healthPanel.SetCurrentHp(health.CurrentHp, health.MaxHp);
            health.OnShieldChanged += healthPanel.SetCurrentShield;
            healthPanel.SetCurrentShield(health.CurrentShield, health.MaxShield);
        }
    }
}