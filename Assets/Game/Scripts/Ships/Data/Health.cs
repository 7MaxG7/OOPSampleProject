namespace Ships.Data
{
    internal sealed class Health : AbstractHealth
    {
        public Health(float hp, float shield, float shieldRecovery, float shieldRecoveryInterval)
        {
            MaxHp = hp;
            ShieldRecovery = shieldRecovery;
            ShieldRecoveryInterval = shieldRecoveryInterval;
            MaxShield = shield;
            RestoreHp();
            RestoreShield();
        }
    }
}