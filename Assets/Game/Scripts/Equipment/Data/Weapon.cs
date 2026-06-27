using System;
using Battle;
using Ships;

namespace Equipment
{
    public sealed class Weapon : IWeapon
    {
        public event Action<IWeapon> OnShoot;
        public event Action<IWeapon> OnUnequip;
        
        public WeaponType WeaponType { get; }
        public bool IsReady => _cooldownTimer <= 0;

        private readonly IDamageHandler _damageHandler;
        
        private IShip _owner;
        
        private readonly int _damage;
        private readonly float _shootCooldown;
        private float _cooldownTimer;

        public Weapon(float cooldown, int damage, WeaponType weaponType, IDamageHandler damageHandler)
        {
            _shootCooldown = cooldown;
            _damage = damage;
            _damageHandler = damageHandler;
            WeaponType = weaponType;
        }

        public void Init(IShip owner)
        {
            _owner = owner;
        }

        public bool TryDealDamageToEnemy(IShip damageTaker)
            => _damageHandler.TryDealDamage(_owner, damageTaker, _damage);

        public void Reload() 
            => _cooldownTimer = 0;

        public void Shoot()
        {
            RestoreCooldown();
            OnShoot?.Invoke(this);
        }

        public void ReduceCooldown(float deltaTime)
            => _cooldownTimer -= Math.Min(deltaTime, _cooldownTimer);

        private void RestoreCooldown() 
            => _cooldownTimer += _shootCooldown;

        public void Unequip()
            => OnUnequip?.Invoke(this);
    }
}