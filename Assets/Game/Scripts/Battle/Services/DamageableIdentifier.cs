using System.Collections.Generic;
using Infrastructure;
using Ships;
using UnityEngine;
using Zenject;

namespace Battle
{
    public sealed class DamageableIdentifier : IDamageableIdentifier
    {
        private readonly Dictionary<IDamageableView, IShip> _ships = new();

        [Inject]
        public DamageableIdentifier(ICleaner cleaner)
        {
            cleaner.AddCleanable(this);
        }

        public void CleanUp()
            => _ships.Clear();

        public void AddShip(IShip ship, IDamageableView view)
            => _ships.TryAdd(view, ship);

        public bool TryGetDamageTaker(Collider2D collider, out IShip damageTaker)
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