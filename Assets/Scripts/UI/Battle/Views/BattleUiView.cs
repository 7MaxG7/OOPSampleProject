using System.Linq;
using Abstractions.Ships;
using Enums;
using TMPro;
using Ui.Battle.Views;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Ui
{
    public sealed class BattleUiView : MonoBehaviour
    {
        [SerializeField] private HealthPanel[] _healthPanels;
        [SerializeField] private TMP_Text _winLable;
        [SerializeField] private Button _leaveButton;

        public Button LeaveButton => _leaveButton;

        
        public HealthPanel GetHealthPanel(OpponentId opponent) 
            => _healthPanels.FirstOrDefault(item => item.OpponentId == opponent);

        public void ShowWinnerLable(IShip winner)
        {
            LeaveButton.gameObject.SetActive(true);
            _winLable.gameObject.SetActive(true);
            _winLable.text = string.Format(Constants.WIN_TEXT, winner.Name);
        }

        public void HideBattleEndObjects()
        {
            _winLable.gameObject.SetActive(false);
            LeaveButton.gameObject.SetActive(false);
        }
    }
}