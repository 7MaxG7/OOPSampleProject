using UnityEngine;
using UnityEngine.UI;

namespace UI.ShipSetup
{
    public sealed class ShipSetupUIView : MonoBehaviour
    {
        [SerializeField] private ShipSetupPanelUIView[] _shipPanels;
        [SerializeField] private EquipmentSelectUIView _weaponSelectPanel;
        [SerializeField] private EquipmentSelectUIView _moduleSelectPanel;
        [SerializeField] private Button _setupCompleteButton;
        [SerializeField] private Button _hideAllButton;

        public ShipSetupPanelUIView[] ShipPanels => _shipPanels;
        public EquipmentSelectUIView WeaponSelectPanel => _weaponSelectPanel;
        public EquipmentSelectUIView ModuleSelectPanel => _moduleSelectPanel;
        public Button SetupCompleteButton => _setupCompleteButton;
        public Button HideAllButton => _hideAllButton;
    }
}