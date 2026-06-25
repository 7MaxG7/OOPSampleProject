using System;
namespace Equipment
{
    public sealed class Module : IModule
    {
        public event Action<IModule> OnUnequip;

        public ModuleType ModuleType { get; }
        public BuffParamType BuffParamType { get; }
        public float Value { get; }
        
        public bool IsReloadRelativeReduce
            => BuffParamType == BuffParamType.ShootCooldown && _buffRelativenessType == BuffRelativenessType.Relative;
        public bool IsShieldRecoveryRelativeSpeedup
            => BuffParamType == BuffParamType.ShieldRecovery && _buffRelativenessType == BuffRelativenessType.Relative;
        public bool IsHpConstantIncrease
            => BuffParamType == BuffParamType.Hp && _buffRelativenessType == BuffRelativenessType.Constant;
        public bool IsShieldConstantIncrease
            => BuffParamType == BuffParamType.Shield && _buffRelativenessType == BuffRelativenessType.Constant;

        private readonly BuffRelativenessType _buffRelativenessType;

        public Module(BuffParamType buffParamType, BuffRelativenessType buffRelativenessType, float value, ModuleType moduleType)
        {
            BuffParamType = buffParamType;
            _buffRelativenessType = buffRelativenessType;
            Value = value;
            ModuleType = moduleType;
        }

        public void Unequip()
            => OnUnequip?.Invoke(this);
    }
}