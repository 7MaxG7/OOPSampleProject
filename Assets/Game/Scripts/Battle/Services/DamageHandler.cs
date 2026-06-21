using Ships;
using UnityEngine;
using Zenject;

namespace Battle
{
    public sealed class DamageHandler : IDamageHandler
    {
        private readonly IDamageableIdentifier _damageableIdentifier;

        [Inject]
        public DamageHandler(IDamageableIdentifier damageableIdentifier)
        {
            _damageableIdentifier = damageableIdentifier;
        }

        public bool TryDealDamage(IShip shooter, Collider2D collider, int damage)
        {
            if (!_damageableIdentifier.TryGetDamageTaker(collider, out var damageTaker) || damageTaker == shooter)
                return false;
            
            damageTaker.TakeDamage(damage);
            return true;
        }
    }
}