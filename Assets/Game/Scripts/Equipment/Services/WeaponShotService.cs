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
        private readonly IAmmoViewFactory _ammoViewFactory;
        private readonly IDamageableIdentifier _damageableIdentifier;
        private readonly IUpdater _updater;

        private readonly Dictionary<IWeapon, WeaponView> _weaponViews = new();
        private readonly Dictionary<AmmoView, IWeapon> _shotAmmoViewWeapons = new();
        private bool _areAmmosDeactivated;

        [Inject]
        public WeaponShotService(IAmmoViewFactory ammoViewFactory, IDamageableIdentifier damageableIdentifier, IUpdater updater)
        {
            _ammoViewFactory = ammoViewFactory;
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

        public void DeactivateShotAmmos()
        {
            foreach (var ammoView in _shotAmmoViewWeapons.Keys)
                StopBullet(ammoView);

            _shotAmmoViewWeapons.Clear();
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

            _areAmmosDeactivated = false;
            var ammoView = await _ammoViewFactory.CreateAmmoViewAsync(weapon.WeaponType);
            if (_areAmmosDeactivated) // Is bullet spawned after fight finished
            {
                ammoView.Deactivate();
                return;
            }

            ShootBullet(weapon, ammoView, weaponView);
        }

        private void ShootBullet(IWeapon weapon, AmmoView ammoView, WeaponView weaponView)
        {
            ammoView.OnTriggerEntered += HandleCollision;

            var barrel = weaponView.Barrel;
            ammoView.Activate(barrel.position, barrel.rotation, barrel.up, weaponView.AmmoSpeed);

            _updater.AddUpdatable(ammoView);
            _shotAmmoViewWeapons.Add(ammoView, weapon);
        }

        private void HandleCollision(Collider2D collider, AmmoView ammoView)
        {
            if (!_shotAmmoViewWeapons.TryGetValue(ammoView, out var weapon))
            {
                Debug.LogError($"{this}: Cannot get weapon for ammo view {ammoView.name}");
                return;
            }

            if (!_damageableIdentifier.TryGetDamageTaker(collider, out var damageTaker))
            {
                Debug.LogError($"{this}: Cannot get damage taker with collider {collider.name}");
                return;
            }

            if (weapon.TryDealDamageToEnemy(damageTaker))
            {
                StopBullet(ammoView);
                _shotAmmoViewWeapons.Remove(ammoView);
            }
        }

        private void StopBullet(AmmoView ammoView)
        {
            if (_areAmmosDeactivated)
                return;

            ammoView.OnTriggerEntered -= HandleCollision;
            ammoView.Deactivate();
            _updater.RemoveUpdatable(ammoView);
        }
    }
}