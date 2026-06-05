using System;
using Infrastructure.ControllersHolder;

namespace Ships
{
    public interface IHealth : IUpdatable
    {
        event Action<float, float> OnHpChanged;
        event Action<float, float> OnShieldChanged;
        
        float ShieldRecovery { get; }
        float MaxHp { get; }
        float MaxShield { get; }
        float CurrentHp { get; }
        float CurrentShield { get; }
        float ShieldRecoveryInterval { get; }

        void RestoreHp();
        void RestoreShield();
        void TakeDamage(int damage);
    }
}