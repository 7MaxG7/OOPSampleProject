using Infrastructure.ControllersHolder;
using Ships;

namespace Battle
{
    public interface IDamageHandler : ISceneCleanable
    {
        void AddShip(IShip ship);
        bool TryDealDamage(IShip shooter, IDamagableView target, int damage);
    }
}