using System;

namespace Ships.Data
{
    public abstract class AbstractHealth : IHealth
    {
        public event Action<float, float> OnHpChanged;
        public event Action<float, float> OnShieldChanged;

        public float ShieldRecovery { get; protected set; }
        public float MaxHp { get; protected set; }
        public float MaxShield { get; protected set; }

        public float CurrentHp
        {
            get => _currentHp;
            private set
            {
                if (_currentHp.Equals(value))
                    return;
                
                _currentHp = value;
                _currentHp = Math.Clamp(value, 0, MaxHp);
                OnHpChanged?.Invoke(_currentHp, MaxHp);
            }
        }

        public float CurrentShield
        {
            get => _currentShield;
            private set
            {
                if (_currentShield.Equals(value))
                    return;
                
                _currentShield = Math.Clamp(value, 0, MaxShield);
                OnShieldChanged?.Invoke(_currentShield, MaxShield);
            }
        }

        public float ShieldRecoveryInterval { get; protected set; }
        private float _shieldRecoverTimer;
        private float _currentHp;
        private float _currentShield;


        public void OnUpdate(float deltaTime)
        {
            _shieldRecoverTimer += deltaTime;
            if (_shieldRecoverTimer < ShieldRecoveryInterval)
                return;

            _shieldRecoverTimer -= ShieldRecoveryInterval;
            RecoverShield(ShieldRecovery);
        }

        public void RestoreHp()
            => CurrentHp = MaxHp;

        public void RestoreShield()
            => CurrentShield = MaxShield;

        public void TakeDamage(int damage)
        {
            var damageLeft = damage - CurrentShield;
            CurrentShield -= damage;

            if (damageLeft > 0)
                CurrentHp -= Math.Min(CurrentHp, damageLeft);
        }

        private void RecoverShield(float deltaShield)
            => CurrentShield += deltaShield;
    }
}