namespace Equipment
{
    public sealed class WeaponBattery : BaseWeaponBattery
    {
        public WeaponBattery(int amount, IWeaponFactory weaponFactory) : base(amount, weaponFactory)
        {
            ReloadRate = 1;
        }
    }
}