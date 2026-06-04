using Cysharp.Threading.Tasks;
using Infrastructure;
using Ships;

namespace Services
{
    public interface IAmmoFactory : ISceneCleanable
    {
        void PrepareRoot();
        UniTask<IAmmo> SpawnAmmo(IWeapon weapon);
    }
}