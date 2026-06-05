using Equipment.Data;

namespace Equipment
{
    public interface IWeaponFactory : IEquipmentFactory<IWeapon, WeaponType>
    {
    }
}