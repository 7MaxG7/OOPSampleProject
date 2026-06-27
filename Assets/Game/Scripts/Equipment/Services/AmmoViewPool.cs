using System.Collections.Generic;
using Ships;

namespace Equipment
{
    public sealed class AmmoViewPool
    {
        private readonly Stack<AmmoView> _ammos = new();
        private readonly HashSet<AmmoView> _spawnedAmmos = new();

        public AmmoView SpawnAmmo()
        {
            if (_ammos.Count == 0)
                return null;

            var ammo = _ammos.Pop();
            RegisterSpawn(ammo);
            return ammo;
        }

        public void RegisterSpawn(AmmoView ammo)
        {
            ammo.OnDeactivated += ReturnObject;
            _spawnedAmmos.Add(ammo);
        }

        public void Clean()
        {
            foreach (var ammoView in _spawnedAmmos)
                ammoView.OnDeactivated -= ReturnObject;
            _spawnedAmmos.Clear();
            _ammos.Clear();
        }

        private void ReturnObject(AmmoView ammo)
        {
            ammo.OnDeactivated -= ReturnObject;
            _spawnedAmmos.Remove(ammo);
            _ammos.Push(ammo);
        }
    }
}
