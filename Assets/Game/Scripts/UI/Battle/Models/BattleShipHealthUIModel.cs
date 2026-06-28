using Cysharp.Threading.Tasks;
using Utils;

namespace UI.Battle
{
    public class BattleShipHealthUIModel
    {
        public AsyncReactiveProperty<(float current, float max)> Hp { get; } = new(default);
        public AsyncReactiveProperty<(float current, float max)> Shield { get; } = new(default);
        
        public void SetHp(float currentHp, float maxHp)
            => Hp.Update((currentHp, maxHp));

        public void SetShield(float currentShield, float maxShield)
            => Shield.Update((currentShield, maxShield));
    }
}