using System.Linq;
using Ships.Data;
using UI.Data;
using UnityEngine;

namespace UI.Ship.Views
{
    public sealed class ShipSlotUiView : SlotUiView
    {
        [SerializeField] private SelectPanelAnchor[] _selectPanelAnchor;
        
        public int Index { get; private set; }
        public Transform SelectPanelAnchor { get; private set; }
        
        public void Init(OpponentId opponentId, int index)
        {
            Index = index;
            SelectPanelAnchor = _selectPanelAnchor.FirstOrDefault(anchor => anchor.OpponentId == opponentId)?.Anchor;
            SetIcon(null);
        }
    }
}