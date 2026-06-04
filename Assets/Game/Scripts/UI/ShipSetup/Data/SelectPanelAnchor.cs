using System;
using Enums;
using UnityEngine;

namespace UI.ShipSetup.Data
{
    [Serializable]
    public sealed class SelectPanelAnchor
    {
        [SerializeField] private OpponentId _opponentId;
        [SerializeField] private Transform _anchor;

        public OpponentId OpponentId => _opponentId;
        public Transform Anchor => _anchor;
    }
}