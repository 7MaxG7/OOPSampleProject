using Cysharp.Threading.Tasks;
using UI;
using UI.Battle;
using UI.Ship;
using UnityEngine;

namespace Ui
{
    public interface IUiFactory
    {
        CurtainView CreateCurtain();
        UniTask CreateRootAsync();
        UniTask<ShipSetupController> CreateShipSetupUIAsync();
        UniTask<BattleUiController> CreateBattleUIAsync();
        UniTask<SlotUiView> CreateSelectEquipmentSlotAsync(Transform parent);
        UniTask<ShipSlotUiView> CreateShipEquipmentSlotAsync(Transform parent);
    }
}
