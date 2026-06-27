namespace Ships
{
    internal sealed class Health : BaseHealth
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