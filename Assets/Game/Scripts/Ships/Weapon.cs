using System;
using Enums;
using Services;
using Ships.Views;
using Object = UnityEngine.Object;

namespace Ships
{
    public sealed class Weapon : IWeapon
    {
        public event Action<IAmmo> OnBulletHit;
        
        public WeaponType WeaponType { get; }
        public bool IsReady => _cooldownTimer <= 0;

        private readonly IAmmoFactory _ammoFactory;
        private readonly IDamageHandler _damageHandler;
        
        private IShip _owner;
        private WeaponView _weaponView;
        
        private readonly int _damage;
        private readonly float _ammoSpeed;
        private readonly float _shootCooldown;
        private float _cooldownTimer;


        public Weapon(float cooldown, int damage, float ammoSpeed, WeaponType weaponType, IAmmoFactory ammoFactory,
            IDamageHandler damageHandler)
        {
            _shootCooldown = cooldown;
            _damage = damage;
            _ammoSpeed = ammoSpeed;
            _ammoFactory = ammoFactory;
            _damageHandler = damageHandler;
            WeaponType = weaponType;
        }

        public void Init(IShip owner)
        {
            _owner = owner;
        }

        public void SetView(WeaponView view)
            => _weaponView = view;

        public void TryDealDamage(IAmmo ammo, IDamagableView target)
        {
            if (_damageHandler.TryDealDamage(_owner, target, _damage))
                OnBulletHit?.Invoke(ammo);
        }

        public void Reload() 
            => _cooldownTimer = 0;

        public async void Shoot()
        {
            var ammo = await _ammoFactory.SpawnAmmo(this);
            ammo.Activate(_weaponView.Barrel, this);
            ammo.Rigidbody.AddForce(_weaponView.Barrel.up * _ammoSpeed);
            RestoreCooldown();
        }

        public void ReduceCooldown(float deltaTime)
            => _cooldownTimer -= Math.Min(deltaTime, _cooldownTimer);

        private void RestoreCooldown() 
            => _cooldownTimer += _shootCooldown;

        public void Unequip()
        {
            Object.Destroy(_weaponView.gameObject);
        }
    }
}