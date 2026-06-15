using Cysharp.Threading.Tasks;
using Equipment;
using Equipment.Data;
using Infrastructure;
using Ships;

namespace UI.Ship
{
    public sealed class WeaponSelectPanelController : BaseEquipmentSelectController<WeaponType>
    {
        public WeaponSelectPanelController(WeaponSelectView weaponSelectView, IShipConfigurator shipConfigurator,
            ICancellationTokenProvider tokenProvider) : base(weaponSelectView, shipConfigurator, tokenProvider) { }

        public async UniTask SetupWeaponSelectPanelAsync(WeaponConfig[] weaponDatas)
        {
            foreach (var data in weaponDatas)
            {
                var button = await EquipmentSelectView.AddEquipmentSelectSlot(data.WeaponType);
                button.onClick.AddListener(() => SelectWeapon(data.WeaponType));
            }
        }

        private void SelectWeapon(WeaponType weaponType)
        {
            ShipConfigurator.SetWeapon(OpponentId, SlotIndex, weaponType);
            HideAsync().Forget();
        }
    }
}
