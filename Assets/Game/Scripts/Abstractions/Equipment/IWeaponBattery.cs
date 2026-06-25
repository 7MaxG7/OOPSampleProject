using System;
using Infrastructure;
using Ships;

namespace Equipment
{
    public interface IWeaponBattery : IEquipmentBattery<IWeapon, WeaponType>, IUpdatable
    {
        event Action<WeaponType> OnShoot;
        float ReloadRate { get; }

        void ToggleShooting(bool isActive);
    }
}
