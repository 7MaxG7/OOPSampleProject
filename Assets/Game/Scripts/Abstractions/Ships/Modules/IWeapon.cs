using System;
using Enums;

namespace Ships
{
    public interface IWeapon : IEquipment
    {
        event Action<IAmmo> OnBulletHit;

        bool IsReady { get; }
        WeaponType WeaponType { get; }

        void Init(IShip owner);
        void Shoot();
        void ReduceCooldown(float deltaTime);
        void TryDealDamage(IAmmo ammo, IDamagableView target);
        void Reload();
    }
}