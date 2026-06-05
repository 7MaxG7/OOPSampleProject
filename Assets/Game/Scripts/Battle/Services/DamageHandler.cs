using System.Collections.Generic;
using Ships;

namespace Battle
{
    public sealed class DamageHandler : IDamageHandler
    {
        private readonly Dictionary<IDamagableView, IShip> _ships = new();
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
            if (_ships.ContainsKey(ship.ShipView))
                return;

            _ships.Add(ship.ShipView, ship);
        }

        public bool TryDealDamage(IShip shooter, IDamagableView target, int damage)
        {
            if (!_ships.TryGetValue(target, out var damageTaker) || damageTaker == shooter)
                return false;
            
            _ships[target].TakeDamage(damage);
            return true;
        }
    }
}