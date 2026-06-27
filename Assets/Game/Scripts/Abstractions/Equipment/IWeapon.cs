using System;
using Ships;

namespace Equipment
{
    public interface IWeapon : IEquipment
    {
        event Action<IWeapon> OnShoot;
        event Action<IWeapon> OnUnequip;

        bool IsReady { get; }
        WeaponType WeaponType { get; }

        void Init(IShip owner);
        void Shoot();
        void ReduceCooldown(float deltaTime);
        bool TryDealDamageToEnemy(IShip damageTaker);
        void Reload();
    }
}