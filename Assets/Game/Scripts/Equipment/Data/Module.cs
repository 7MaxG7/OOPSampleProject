using System;
using Ships;

namespace Equipment
{
    public sealed class Module : IModule
    {
        public event Action<IModule> OnUnequip;

        public ModuleType ModuleType { get; }
        public BuffParamType BuffParamType { get; }

        private readonly float _value;
        private readonly BuffRelativenessType _buffRelativenessType;

        public Module(BuffParamType buffParamType, BuffRelativenessType buffRelativenessType, float value, ModuleType moduleType)
        {
            BuffParamType = buffParamType;
            _buffRelativenessType = buffRelativenessType;
            _value = value;
            ModuleType = moduleType;
        }

        public void Unequip()
            => OnUnequip?.Invoke(this);

        public void UpdateParams(UpgradedWeaponsBattery weaponsBattery)
        {
            weaponsBattery.ReloadRate = CalculateUpgradedParam(BuffParamType.ShootCooldown, weaponsBattery.BaseWeaponBattery.ReloadRate);
        }

        public void UpdateParams(UpgradedHealth health)
        {
            health.ShieldRecovery = CalculateUpgradedParam(BuffParamType.ShieldRecovery, health.BaseHealth.ShieldRecovery);
            health.MaxHp = CalculateUpgradedParam(BuffParamType.Hp, health.BaseHealth.MaxHp);
            health.MaxShield = CalculateUpgradedParam(BuffParamType.Shield, health.BaseHealth.MaxShield);
        }

        private float CalculateUpgradedParam(BuffParamType buffParamType, float baseParamValue)
            => BuffParamType == buffParamType
                ? CalculateUpgradedParam(baseParamValue)
                : baseParamValue;

        private float CalculateUpgradedParam(float baseValue)
            => _buffRelativenessType switch
            {
                BuffRelativenessType.Relative => baseValue * _value,
                BuffRelativenessType.Constant => baseValue + _value,
                _ => baseValue,
            };
    }
}