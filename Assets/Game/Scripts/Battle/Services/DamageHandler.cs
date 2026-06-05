using System.Collections.Generic;
using Ships;
using UnityEngine;

namespace Battle
{
    public sealed class DamageHandler : IDamageHandler
    {
        private readonly Dictionary<IDamageableView, IShip> _ships = new();
        private bool _isCleaned = true;

        public void CleanUp() 
            => SceneCleanUp();

        public void SceneCleanUp()
        {
            if (_isCleaned)
                return;
            _isCleaned = true;
            
            _ships.Clear();
        }

        public void AddShip(IShip ship)
        {
            _isCleaned = false;
            _ships.TryAdd(ship.ShipView, ship);
        }

        public bool TryDealDamage(IShip shooter, Collider2D collider, int damage)
        {
            if (!TryGetDamageTaker(collider, out var damageTaker) || damageTaker == shooter)
                return false;
            
            damageTaker.TakeDamage(damage);
            return true;
        }

        private bool TryGetDamageTaker(Collider2D collider, out IShip damageTaker)
        {
            foreach (var (view, ship) in _ships)
            foreach (var damageCollider in view.DamageColliders)
            {
                if (damageCollider != collider)
                    continue;

                damageTaker = ship;
                return true;
            }

            damageTaker = null;
            return false;
        }
    }
}