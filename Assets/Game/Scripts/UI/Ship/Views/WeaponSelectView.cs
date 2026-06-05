using Cysharp.Threading.Tasks;
using Equipment.Data;

namespace UI.Ship.Views
{
    public sealed class WeaponSelectView : AbstractEquipmentSelectView<WeaponType>
    {
        protected override async UniTask<SlotUiView> CreateSelectUiSlot(WeaponType weaponType) 
            => await UiFactory.CreateSelectWeaponUiSlotAsync(weaponType, EquipmentsContent);
    }
}
