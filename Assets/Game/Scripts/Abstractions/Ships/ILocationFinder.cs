using Ships.Data;
using UnityEngine;

namespace Ships
{
    public interface ILocationFinder
    {
        Vector3? GetOpponentLocation(OpponentId opponentId, out Quaternion rotation);
    }
}