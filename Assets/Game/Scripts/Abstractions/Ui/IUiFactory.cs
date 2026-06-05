using Cysharp.Threading.Tasks;
using Equipment.Data;
using UI;
using UI.Battle;
using UI.Ship;
using UI.Ship.Views;
using UnityEngine;

namespace Ui
{
    public interface IUiFactory
    {
        UniTask PrepareCanvasAsync();
        UniTask<CurtainView> CreateCurtainAsync();
        UniTask<ShipSetupMenuController> CreateShipSetupMenuAsync();
        UniTask<BattleUiController> CreateBattleUiAsync();
        UniTask<SlotUiView> CreateSelectWeaponUiSlotAsync(WeaponType weaponType, Transform parent);
        UniTask<SlotUiView> CreateSelectModuleUiSlotAsync(ModuleType moduleType, Transform parent);
        UniTask<ShipSlotUiView> CreateEquipmentUiSlotAsync(Transform parent);
    }
}
