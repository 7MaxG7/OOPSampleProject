using Ships;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI.Battle.Views
{
    public sealed class BattleUiView : MonoBehaviour
    {
        [SerializeField] private HealthPanel[] _healthPanels;
        [SerializeField] private TMP_Text _winLable;
        [SerializeField] private Button _leaveButton;

        public Button LeaveButton => _leaveButton;
        public HealthPanel[] HealthPanels => _healthPanels;

        public void Init()
        {
            _winLable.gameObject.SetActive(false);
            LeaveButton.gameObject.SetActive(false);
        }

        public void ShowWinnerLabel(IShip winner)
        {
            LeaveButton.gameObject.SetActive(true);
            _winLable.gameObject.SetActive(true);
            _winLable.text = string.Format(Constants.WIN_TEXT, winner.Name);
        }
    }
}