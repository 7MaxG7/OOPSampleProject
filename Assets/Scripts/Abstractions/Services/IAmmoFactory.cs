using Cysharp.Threading.Tasks;
using Abstractions.Infrastructure;
using Abstractions.Ships;

namespace Abstractions.Services
{
    public interface IAmmoFactory : ISceneCleanable
    {
        void PrepareRoot();
        UniTask<IAmmo> SpawnAmmo(IWeapon weapon);
    }
}