using System;
using Ships;
using UnityEngine;

namespace Equipment
{
    public interface IWeapon : IEquipment
    {
        event Action<IAmmo> OnBulletHit;
        event Action<IWeapon> OnShoot;
        event Action<IWeapon> OnUnequip;

        bool IsReady { get; }
        WeaponType WeaponType { get; }

        void Init(IShip owner);
        void Shoot();
        void ReduceCooldown(float deltaTime);
        void TryDealDamage(IAmmo ammo, Collider2D collider);
        void Reload();
    }
}