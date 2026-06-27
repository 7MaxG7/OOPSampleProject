using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Infrastructure;
using Ships;
using UnityEngine;
using Zenject;

namespace Equipment
{
    public sealed class WeaponShotService : IWeaponShotService
    {
        private readonly IAmmoFactory _ammoFactory;
        private readonly IUpdater _updater;

        private readonly Dictionary<IWeapon, WeaponView> _weaponViews = new();
        private readonly Dictionary<IAmmo, AmmoView> _shotAmmoViews = new();
        private bool _areAmmosDeactivated;

        [Inject]
        public WeaponShotService(IAmmoFactory ammoFactory, IUpdater updater)
        {
            _ammoFactory = ammoFactory;
            _updater = updater;
        }

        public void RegisterWeapon(IWeapon weapon, WeaponView weaponView)
        {
            _weaponViews[weapon] = weaponView;
            weapon.OnShoot += Shoot;
            weapon.OnBulletHit += StopBullet;
        }

        public void UnregisterWeapon(IWeapon weapon)
        {
            weapon.OnShoot -= Shoot;
            _weaponViews.Remove(weapon);
            weapon.OnBulletHit -= StopBullet;
        }

        public void DeactivateShotAmmos()
        {
            foreach (var ammo in _shotAmmoViews.Keys)
                if (_shotAmmoViews.TryGetValue(ammo, out var ammoView))
                {
                    ammoView.Deactivate();
                    _updater.RemoveUpdatable(ammoView);
                }

            _shotAmmoViews.Clear();
            _areAmmosDeactivated = true;
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

            var ammo = await _ammoFactory.SpawnAmmoAsync(weapon);
            if (ammo == null)
                return;

            var barrel = weaponView.Barrel;
            ammo.Activate(barrel.position, barrel.rotation, barrel.up, weaponView.AmmoSpeed, weapon);
            _updater.AddUpdatable(ammo.AmmoView);
            _shotAmmoViews.Add(ammo, ammo.AmmoView);
            _areAmmosDeactivated = false;
        }

        private void StopBullet(IAmmo ammo)
        {
            if (_areAmmosDeactivated)
                return;

            if (!_shotAmmoViews.Remove(ammo, out var ammoView))
            {
                Debug.LogError($"{this}: Cannot get ammo view");
                return;
            }

            _updater.RemoveUpdatable(ammoView);
        }
    }
}
