using Cysharp.Threading.Tasks;
using Infrastructure;
using Ships;

namespace Equipment
{
    public interface IAmmoViewFactory : ISceneCleanable
    {
        UniTask<AmmoView> CreateAmmoViewAsync(WeaponType weaponType);
    }
}
