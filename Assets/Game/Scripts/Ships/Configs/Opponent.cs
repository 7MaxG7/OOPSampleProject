using System;
using UnityEngine;

namespace Ships
{
    [Serializable]
    public class Opponent
    {
        [SerializeField] private OpponentId _opponentId;
        [SerializeField] private ShipType _shipType;

        public OpponentId OpponentId => _opponentId;
        public ShipType ShipType => _shipType;
    }
}