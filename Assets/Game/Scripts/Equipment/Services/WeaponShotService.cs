using System.Collections.Generic;
using Battle;
using Cysharp.Threading.Tasks;
using Infrastructure;
using Ships;
using UnityEngine;
using Zenject;

namespace Equipment
{
    public sealed class WeaponShotService : IWeaponShotService
    {
        private readonly IBulletViewFactory _bulletViewFactory;
        private readonly IDamageableIdentifier _damageableIdentifier;
        private readonly IUpdater _updater;

        private readonly Dictionary<IWeapon, WeaponView> _weaponViews = new();
        private readonly Dictionary<BulletView, IWeapon> _shotBulletViewWeapons = new();
        private bool _areBulletsDeactivated;

        [Inject]
        public WeaponShotService(IBulletViewFactory bulletViewFactory, IDamageableIdentifier damageableIdentifier, IUpdater updater)
        {
            _bulletViewFactory = bulletViewFactory;
            _damageableIdentifier = damageableIdentifier;
            _updater = updater;
        }

        public void RegisterWeapon(IWeapon weapon, WeaponView weaponView)
        {
            _weaponViews[weapon] = weaponView;
            weapon.OnShoot += Shoot;
        }

        public void UnregisterWeapon(IWeapon weapon)
        {
            weapon.OnShoot -= Shoot;
            _weaponViews.Remove(weapon);
        }

        public void DeactivateShotBullets()
        {
            foreach (var bulletView in _shotBulletViewWeapons.Keys)
                StopBullet(bulletView);

            _shotBulletViewWeapons.Clear();
            _areBulletsDeactivated = true;
        }

        private void Shoot(IWeapon weapon)
            => ShootAsync(weapon).Forget();

        private async UniTaskVoid ShootAsync(IWeapon weapon)
        {
            if (!_weaponViews.TryGetValue(weapon, out var weaponView))
            {
                Debug.LogError($"{this}: Cannot get weapon {weapon.WeaponType} view");
                return;
            }

            _areBulletsDeactivated = false;
            var bulletView = await _bulletViewFactory.CreateBulletViewAsync(weapon.WeaponType);
            if (_areBulletsDeactivated) // Is bullet spawned after fight finished
            {
                bulletView.Deactivate();
                return;
            }

            ShootBullet(weapon, bulletView, weaponView);
        }

        private void ShootBullet(IWeapon weapon, BulletView bulletView, WeaponView weaponView)
        {
            bulletView.OnTriggerEntered += HandleCollision;

            var barrel = weaponView.Barrel;
            bulletView.Activate(barrel.position, barrel.rotation, barrel.up, weaponView.BulletSpeed);

            _updater.AddUpdatable(bulletView);
            _shotBulletViewWeapons.Add(bulletView, weapon);
        }

        private void HandleCollision(Collider2D collider, BulletView bulletView)
        {
            if (!_shotBulletViewWeapons.TryGetValue(bulletView, out var weapon))
            {
                Debug.LogError($"{this}: Cannot get weapon for bullet view {bulletView.name}");
                return;
            }

            if (!_damageableIdentifier.TryGetDamageTaker(collider, out var damageTaker))
            {
                Debug.LogError($"{this}: Cannot get damage taker with collider {collider.name}");
                return;
            }

            if (weapon.TryDealDamageToEnemy(damageTaker))
            {
                StopBullet(bulletView);
                _shotBulletViewWeapons.Remove(bulletView);
            }
        }

        private void StopBullet(BulletView bulletView)
        {
            if (_areBulletsDeactivated)
                return;

            bulletView.OnTriggerEntered -= HandleCollision;
            bulletView.Deactivate();
            _updater.RemoveUpdatable(bulletView);
        }
    }
}
