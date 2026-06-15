using System;
using Equipment.Data;
using Infrastructure;
using Ships;

namespace Equipment
{
    public interface IWeaponBattery : IEquipments<IWeapon, WeaponType>, IUpdatable
    {
        event Action<WeaponType> OnShoot;
        float ReloadRate { get; }

        void Init(IShip ship);
        void ToggleShooting(bool isActive);
    }
}
