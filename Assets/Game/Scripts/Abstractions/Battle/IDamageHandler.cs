using Ships;

namespace Battle
{
    public interface IDamageHandler
    {
        bool TryDealDamage(IShip shooter, IShip damageTaker, int damage);
    }
}