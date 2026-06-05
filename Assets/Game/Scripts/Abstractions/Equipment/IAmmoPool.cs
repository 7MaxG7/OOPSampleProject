using Infrastructure.ControllersHolder;

namespace Equipment
{
    public interface IAmmoPool : ICleanable
    {
        IAmmo SpawnAmmo(IWeapon weapon);
        void RegisterAsSpawned(IAmmo ammo, IWeapon weapon);
    }
}