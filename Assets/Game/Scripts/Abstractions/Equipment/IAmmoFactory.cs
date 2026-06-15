using Cysharp.Threading.Tasks;
using Infrastructure;

namespace Equipment
{
    public interface IAmmoFactory : ISceneCleanable
    {
        UniTask<IAmmo> SpawnAmmoAsync(IWeapon weapon);
    }
}