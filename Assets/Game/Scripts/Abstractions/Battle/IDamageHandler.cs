using Ships;
using UnityEngine;

namespace Battle
{
    public interface IDamageHandler
    {
        bool TryDealDamage(IShip shooter, Collider2D collider, int damage);
    }
}