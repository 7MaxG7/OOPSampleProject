using UI.Data;
using UnityEngine;

namespace UI.Ship
{
    public sealed class ShipSlotUiView : SlotUiView
    {
        [SerializeField] private SelectPanelAnchor[] _selectPanelAnchor;
        
        public SelectPanelAnchor[] SelectPanelAnchor => _selectPanelAnchor;
    }
}