using UnityEngine;

namespace UI.ShipSetup
{
    public sealed class ShipSlotUiView : SlotUiView
    {
        [SerializeField] private SelectPanelAnchor[] _selectPanelAnchor;
        
        public SelectPanelAnchor[] SelectPanelAnchor => _selectPanelAnchor;
    }
}