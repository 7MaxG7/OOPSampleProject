using Enums;
using UnityEngine;

namespace Services
{
    public interface ILocationFinder
    {
        Vector3? GetOpponentLocation(OpponentId opponentId, out Quaternion rotation);
    }
}