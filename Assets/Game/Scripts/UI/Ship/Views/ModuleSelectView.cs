using Cysharp.Threading.Tasks;
using Equipment.Data;

namespace UI.Ship
{
    public sealed class ModuleSelectView : BaseEquipmentSelectView<ModuleType>
    {
        protected override async UniTask<SlotUiView> CreateSelectUiSlot(ModuleType moduleType) 
            => await UiFactory.CreateSelectModuleUiSlotAsync(moduleType, EquipmentsContent);
    }
}
