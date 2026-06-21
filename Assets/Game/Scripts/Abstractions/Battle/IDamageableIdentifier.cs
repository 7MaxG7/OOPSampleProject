using Infrastructure;
using Ships;
using UnityEngine;

namespace Battle
{
    public interface IDamageableIdentifier : ISceneCleanable
    {
        void AddShip(IShip ship, IDamageableView view);
        bool TryGetDamageTaker(Collider2D collider, out IShip damageTaker);
    }
}