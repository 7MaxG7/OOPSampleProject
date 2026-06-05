using Ships.Data;
using UnityEngine;

namespace UI.Ship.Views
{
    public sealed class ShipSetupPanelView : MonoBehaviour
    {
        [SerializeField] private OpponentId _opponentId;
        [SerializeField] private Transform _weaponSlotsContent;
        [SerializeField] private Transform _moduleSlotsContent;

        public OpponentId OpponentId => _opponentId;
        public Transform WeaponSlotsContent => _weaponSlotsContent;
        public Transform ModuleSlotsContent => _moduleSlotsContent;
    }
}