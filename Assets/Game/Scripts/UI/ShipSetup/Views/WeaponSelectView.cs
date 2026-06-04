using Cysharp.Threading.Tasks;
using Enums;

namespace UI.ShipSetup
{
    public sealed class WeaponSelectView : AbstractEquipmentSelectView<WeaponType>
    {
        protected override async UniTask<SlotUiView> CreateSelectUiSlot(WeaponType weaponType) 
            => await UiFactory.CreateSelectWeaponUiSlotAsync(weaponType, EquipmentsContent);
    }
}
