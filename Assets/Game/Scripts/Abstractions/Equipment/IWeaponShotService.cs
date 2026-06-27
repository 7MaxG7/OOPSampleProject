using Ships;

namespace Equipment
{
    public interface IWeaponShotService
    {
        void RegisterWeapon(IWeapon weapon, WeaponView weaponView);
        void UnregisterWeapon(IWeapon weapon);
        void DeactivateShotAmmos();
    }
}