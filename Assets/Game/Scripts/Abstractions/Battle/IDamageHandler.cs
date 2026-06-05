using Infrastructure.ControllersHolder;
using Ships;
using UnityEngine;

namespace Battle
{
    public interface IDamageHandler : ISceneCleanable
    {
        void AddShip(IShip ship);
        bool TryDealDamage(IShip shooter, Collider2D collider, int damage);
    }
}