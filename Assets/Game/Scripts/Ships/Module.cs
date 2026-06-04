using System;
using Enums;
using Ships.Views;
using Object = UnityEngine.Object;

namespace Ships
{
    public sealed class Module : IModule
    {
        public event Action<IModule> OnModuleUnequip;

        public ModuleType ModuleType { get; }
        public EffectType EffectType { get; }
        public float Value { get; }
        
        public bool IsReloadRelativeReduce
            => EffectType == EffectType.ShootCooldown && _mathType == MathType.Relative;
        public bool IsShieldRecoveryRelativeSpeedup
            => EffectType == EffectType.ShieldRecovery && _mathType == MathType.Relative;
        public bool IsHpConstantIncrease
            => EffectType == EffectType.Hp && _mathType == MathType.Constant;
        public bool IsShieldConstantIncrease
            => EffectType == EffectType.Shield && _mathType == MathType.Constant;

        private readonly MathType _mathType;
        private ModuleView _moduleView;


        public Module(EffectType effectType, MathType mathType, float value, ModuleType moduleType)
        {
            EffectType = effectType;
            _mathType = mathType;
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