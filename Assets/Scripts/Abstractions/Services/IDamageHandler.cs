using Abstractions.Infrastructure;
using Abstractions.Ships;

namespace Abstractions.Services
{
    public interface IDamageHandler : ISceneCleanable
    {
        void AddShip(IShip ship);
        bool TryDealDamage(IShip shooter, IDamagableView target, int damage);
    }
}