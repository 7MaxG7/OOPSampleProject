using Cysharp.Threading.Tasks;
using Enums;

namespace UI.ShipSetup
{
    public sealed class ModuleSelectView : AbstractEquipmentSelectView<ModuleType>
    {
        protected override async UniTask<SlotUiView> CreateSelectUiSlot(ModuleType moduleType) 
            => await UiFactory.CreateSelectModuleUiSlotAsync(moduleType, EquipmentsContent);
    }
}
