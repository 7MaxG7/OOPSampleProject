using Infrastructure;

namespace Equipment
{
    public interface IAmmoPool : ICleanable
    {
        IAmmo SpawnAmmo(IWeapon weapon);
        void RegisterSpawn(IAmmo ammo, IWeapon weapon);
    }
}