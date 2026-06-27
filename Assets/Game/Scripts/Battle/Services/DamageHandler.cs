using Ships;

namespace Battle
{
    public sealed class DamageHandler : IDamageHandler
    {
        public bool TryDealDamage(IShip shooter, IShip damageTaker, int damage)
        {
            if (damageTaker == shooter)
                return false;
            
            damageTaker.TakeDamage(damage);
            return true;
        }
    }
}