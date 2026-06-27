using UnityEngine;

namespace Ships
{
    public sealed class ShipSpawnerMarker : MonoBehaviour
    {
        [SerializeField] private OpponentId _opponentId;

        public OpponentId OpponentId => _opponentId;
    }
}