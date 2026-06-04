using System;
using System.Collections.Generic;
using Enums;
using UnityEngine;

namespace Configs.Data
{
    [Serializable]
    public class Opponent
    {
        [SerializeField] private OpponentId _opponentId;
        [SerializeField] private ShipType _shipType;
        [SerializeField] private List<SpawnPosition> _spawnPositions;

        public OpponentId OpponentId => _opponentId;
        public ShipType ShipType => _shipType;
        public List<SpawnPosition> SpawnPositions => _spawnPositions;
    }
}