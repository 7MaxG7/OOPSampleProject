using Cysharp.Threading.Tasks;
using Enums;
using UI;
using UI.Battle;
using UI.ShipSetup;
using UnityEngine;

namespace Services
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
