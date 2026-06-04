using Ui.ShipSetup;
using UnityEngine;
using UnityEngine.UI;

namespace Ui
{
    public sealed class ShipSetupMenuView : MonoBehaviour
    {
        [SerializeField] private ShipSetupPanelView[] _shipPanels;
        [SerializeField] private WeaponSelectView _weaponSelectPanel;
        [SerializeField] private ModuleSelectView _moduleSelectPanel;
        [SerializeField] private Button _setupCompleteButton;
        [SerializeField] private Button _hideAllButton;

        public ShipSetupPanelView[] ShipPanels => _shipPanels;
        public WeaponSelectView WeaponSelectPanel => _weaponSelectPanel;
        public ModuleSelectView ModuleSelectPanel => _moduleSelectPanel;
        public Button SetupCompleteButton => _setupCompleteButton;
        public Button HideAllButton => _hideAllButton;
    }
}