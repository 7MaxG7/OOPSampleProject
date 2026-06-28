using UnityEngine;

namespace UI.ShipSetup
{
    public sealed class ShipSlotUIView : SlotUIView
    {
        [SerializeField] private SelectPanelAnchor[] _selectPanelAnchor;
        
        public SelectPanelAnchor[] SelectPanelAnchor => _selectPanelAnchor;
    }
}