using Cysharp.Threading.Tasks;
using Infrastructure;
using Ships;

namespace Equipment
{
    public interface IBulletViewFactory : ISceneCleanable
    {
        UniTask<BulletView> CreateBulletViewAsync(WeaponType weaponType);
    }
}
