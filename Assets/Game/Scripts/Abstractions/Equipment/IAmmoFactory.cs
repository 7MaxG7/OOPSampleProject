using Cysharp.Threading.Tasks;
using Infrastructure.ControllersHolder;

namespace Equipment
{
    public interface IAmmoFactory : ISceneCleanable
    {
        UniTask<IAmmo> SpawnAmmoAsync(IWeapon weapon);
    }
}