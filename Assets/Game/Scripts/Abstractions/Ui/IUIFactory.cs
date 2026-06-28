using Cysharp.Threading.Tasks;
using UI;
using UI.Battle.Views;
using UI.ShipSetup;
using UnityEngine;

namespace Ui
{
    public interface IUIFactory
    {
        CurtainUIView CreateCurtain();
        UniTask CreateRootAsync();
        UniTask<ShipSetupUIView> CreateShipSetupUIAsync();
        UniTask<BattleUIView> CreateBattleUIAsync();
        UniTask<SlotUIView> CreateSelectEquipmentSlotAsync(Transform parent);
        UniTask<ShipSlotUIView> CreateShipEquipmentSlotAsync(Transform parent);
    }
}
