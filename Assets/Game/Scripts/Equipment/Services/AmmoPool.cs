using System.Collections.Generic;

namespace Equipment
{
    public sealed class AmmoPool : IAmmoPool
    {
        private readonly Stack<IAmmo> _ammos = new();
        private readonly List<IAmmo> _spawnedAmmos = new();
        private readonly HashSet<IWeapon> _weapons = new();

        private bool _isCleaned = true;


        public void CleanUp()
        {
            if (_isCleaned)
                return;
            _isCleaned = true;

            foreach (var weapon in _weapons) 
                weapon.OnBulletHit -= ReturnObject;
            _weapons.Clear();

            foreach (var ammo in _spawnedAmmos) 
                ammo.CleanUp();
            _spawnedAmmos.Clear();
            
            foreach (var ammo in _ammos) 
                ammo.CleanUp();
            _ammos.Clear();
        }

        public IAmmo SpawnAmmo(IWeapon weapon)
        {
            if (_ammos.Count == 0)
                return null;

            var ammo = _ammos.Pop();
            RegisterAsSpawned(ammo, weapon);
            return ammo;
        }

        public void RegisterAsSpawned(IAmmo ammo, IWeapon weapon)
        {
            _isCleaned = false;
            if (_weapons.Add(weapon))
                weapon.OnBulletHit += ReturnObject;
            _spawnedAmmos.Add(ammo);
        }

        private void ReturnObject(IAmmo ammo)
        {
            _spawnedAmmos.Remove(ammo);
            ammo.Deactivate();
            _ammos.Push(ammo);
        }
    }
}