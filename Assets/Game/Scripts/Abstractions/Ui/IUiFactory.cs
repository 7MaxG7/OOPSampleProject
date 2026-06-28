using Cysharp.Threading.Tasks;
using UI;
using UI.Battle.Views;
using UI.ShipSetup;
using UnityEngine;

namespace Ui
{
    public interface IUiFactory
    {
        CurtainView CreateCurtain();
        UniTask CreateRootAsync();
        UniTask<ShipSetupView> CreateShipSetupUIAsync();
        UniTask<BattleUiView> CreateBattleUIAsync();
        UniTask<SlotUiView> CreateSelectEquipmentSlotAsync(Transform parent);
        UniTask<ShipSlotUiView> CreateShipEquipmentSlotAsync(Transform parent);
    }
}
