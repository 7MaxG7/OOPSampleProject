using UnityEngine;
using UnityEngine.UI;

namespace UI.Ship
{
    public sealed class ShipSetupView : MonoBehaviour
    {
        [SerializeField] private ShipSetupPanelUIView[] _shipPanels;
        [SerializeField] private EquipmentSelectView _weaponSelectPanel;
        [SerializeField] private EquipmentSelectView _moduleSelectPanel;
        [SerializeField] private Button _setupCompleteButton;
        [SerializeField] private Button _hideAllButton;

        public ShipSetupPanelUIView[] ShipPanels => _shipPanels;
        public EquipmentSelectView WeaponSelectPanel => _weaponSelectPanel;
        public EquipmentSelectView ModuleSelectPanel => _moduleSelectPanel;
        public Button SetupCompleteButton => _setupCompleteButton;
        public Button HideAllButton => _hideAllButton;
    }
}