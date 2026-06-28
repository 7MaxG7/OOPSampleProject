using Ships;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI.Battle.Views
{
    public sealed class BattleUIView : MonoBehaviour
    {
        [SerializeField] private HealthPanelUIView[] _healthPanels;
        [SerializeField] private TMP_Text _winLabel;
        [SerializeField] private Button _leaveButton;

        public Button LeaveButton => _leaveButton;
        public HealthPanelUIView[] HealthPanels => _healthPanels;

        public void Init()
        {
            _winLabel.gameObject.SetActive(false);
            LeaveButton.gameObject.SetActive(false);
        }

        public void ShowWinnerLabel(IShip winner)
        {
            LeaveButton.gameObject.SetActive(true);
            _winLabel.gameObject.SetActive(true);
            _winLabel.text = string.Format(Constants.WIN_TEXT, winner.Name);
        }
    }
}