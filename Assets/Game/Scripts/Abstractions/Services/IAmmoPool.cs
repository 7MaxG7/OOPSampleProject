using Infrastructure;
using Ships;

namespace Services
{
    public interface IAmmoPool : ICleanable
    {
        IAmmo SpawnAmmo(IWeapon weapon);
        void RegisterAsSpawned(IAmmo ammo, IWeapon weapon);
    }
}