using System;
using Enums;
using UnityEngine;

namespace Ui.ShipSetup.Data
{
    [Serializable]
    public sealed class OpponentAnchor
    {
        [SerializeField] private OpponentId _opponentId;
        [SerializeField] private Vector2 _min;
        [SerializeField] private Vector2 _max;
        [SerializeField] private Vector2 _pivot;

        public OpponentId OpponentId => _opponentId;
        public Vector2 Min => _min;
        public Vector2 Max => _max;
        public Vector2 Pivot => _pivot;
    }
}