using Enums;
using Ships;

namespace Services
{
    public interface IWeaponFactory : IEquipmentFactory<IWeapon, WeaponType>
    {
    }
}