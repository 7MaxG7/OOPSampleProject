using System;
using Equipment.Data;

namespace Equipment
{
    public interface IModule : IEquipment
    {
        event Action<IModule> OnModuleUnequip;
        
        ModuleType ModuleType { get; }
        BuffParamType BuffParamType { get; }
        float Value { get; }
        
        bool IsReloadRelativeReduce { get; }
        bool IsShieldRecoveryRelativeSpeedup { get; }
        bool IsHpConstantIncrease { get; }
        bool IsShieldConstantIncrease { get; }
    }
}