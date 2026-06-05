using System;
using Ships.Views;
using Object = UnityEngine.Object;

namespace Equipment.Data
{
    public sealed class Module : IModule
    {
        public event Action<IModule> OnModuleUnequip;

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
        private ModuleView _moduleView;


        public Module(BuffParamType buffParamType, BuffRelativenessType buffRelativenessType, float value, ModuleType moduleType)
        {
            BuffParamType = buffParamType;
            _buffRelativenessType = buffRelativenessType;
            Value = value;
            ModuleType = moduleType;
        }

        public void SetView(ModuleView view)
            => _moduleView = view;

        public void Unequip()
        {
            OnModuleUnequip?.Invoke(this);
            Object.Destroy(_moduleView.gameObject);
        }
    }
}