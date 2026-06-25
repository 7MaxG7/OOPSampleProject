using Ships;

namespace Equipment
{
    public interface IEquipmentIdentifier
    {
        bool TryGetModuleOwner(IModule module, out IShip ship);
        bool TryGetWeaponOwner(IWeapon weapon, out IShip ship);
    }
}