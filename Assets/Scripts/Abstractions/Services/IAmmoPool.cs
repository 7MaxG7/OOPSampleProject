using Abstractions.Infrastructure;
using Abstractions.Ships;

namespace Abstractions.Services
{
    public interface IAmmoPool : ICleanable
    {
        IAmmo SpawnAmmo(IWeapon weapon);
        void RegisterAsSpawned(IAmmo ammo, IWeapon weapon);
    }
}