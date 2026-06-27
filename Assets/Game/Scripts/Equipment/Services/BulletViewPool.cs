using System.Collections.Generic;
using Ships;

namespace Equipment
{
    public sealed class BulletViewPool
    {
        private readonly Stack<BulletView> _bullets = new();
        private readonly HashSet<BulletView> _spawnedBullets = new();

        public BulletView SpawnBullet()
        {
            if (_bullets.Count == 0)
                return null;

            var bullet = _bullets.Pop();
            RegisterSpawn(bullet);
            return bullet;
        }

        public void RegisterSpawn(BulletView bullet)
        {
            bullet.OnDeactivated += ReturnObject;
            _spawnedBullets.Add(bullet);
        }

        public void Clean()
        {
            foreach (var bulletView in _spawnedBullets)
                bulletView.OnDeactivated -= ReturnObject;
            _spawnedBullets.Clear();
            _bullets.Clear();
        }

        private void ReturnObject(BulletView bullet)
        {
            bullet.OnDeactivated -= ReturnObject;
            _spawnedBullets.Remove(bullet);
            _bullets.Push(bullet);
        }
    }
}
