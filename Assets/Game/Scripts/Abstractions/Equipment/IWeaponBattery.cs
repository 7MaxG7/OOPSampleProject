using System;
using Infrastructure;

namespace Equipment
{
    public interface IWeaponBattery : IEquipmentBattery<IWeapon, WeaponType>, IUpdatable
    {
        event Action<WeaponType> OnShoot;
        float ReloadRate { get; }

        void ToggleShooting(bool isActive);
    }
}
