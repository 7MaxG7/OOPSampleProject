using Infrastructure;
using Ships;

namespace Services
{
    public interface IDamageHandler : ISceneCleanable
    {
        void AddShip(IShip ship);
        bool TryDealDamage(IShip shooter, IDamagableView target, int damage);
    }
}