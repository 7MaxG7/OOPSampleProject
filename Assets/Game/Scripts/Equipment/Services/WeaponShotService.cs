using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Ships;
using Zenject;

namespace Equipment
{
    public sealed class WeaponShotService : IWeaponShotService
    {
        private readonly IAmmoFactory _ammoFactory;
        
        private readonly Dictionary<IWeapon, WeaponView> _weaponViews = new();

        [Inject]
        public WeaponShotService(IAmmoFactory ammoFactory)
        {
            _ammoFactory = ammoFactory;
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

        private void Shoot(IWeapon weapon)
            => ShootAsync(weapon).Forget();

        private async UniTaskVoid ShootAsync(IWeapon weapon)
        {
            var ammo = await _ammoFactory.SpawnAmmoAsync(weapon);
            
            var weaponView = _weaponViews[weapon];
            ammo.Activate(weaponView.Barrel, weapon);
            ammo.Rigidbody.AddForce(weaponView.Barrel.up * weaponView.AmmoSpeed);
        }
    }
}