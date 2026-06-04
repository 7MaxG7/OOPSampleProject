using Cysharp.Threading.Tasks;
using Enums;
using Ui;
using Ui.Battle;
using Ui.ShipSetup;
using UI.ShipSetup;
using UnityEngine;

namespace Abstractions.Services
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
