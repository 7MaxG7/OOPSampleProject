using Cysharp.Threading.Tasks;
using Infrastructure.ControllersHolder;

namespace Equipment
{
    public interface IAmmoFactory : ISceneCleanable
    {
        void PrepareRoot();
        UniTask<IAmmo> SpawnAmmo(IWeapon weapon);
    }
}