using System.Collections.Generic;
using Configs.Data;
using Cysharp.Threading.Tasks;
using Enums;
using Infrastructure;
using Ships;

namespace UI.ShipSetup
{
    public sealed class WeaponSelectPanelController : AbstractEquipmentSelectController<WeaponType>
    {
        public WeaponSelectPanelController(WeaponSelectView weaponSelectView, Dictionary<OpponentId, ShipModel> shipModels,
            ICancellationTokenProvider tokenProvider) : base(weaponSelectView, shipModels, tokenProvider) { }

        public async UniTask SetupWeaponSelectPanelAsync(WeaponData[] weaponDatas)
        {
            foreach (var data in weaponDatas)
            {
                var button = await EquipmentSelectView.AddEquipmentSelectSlot(data.WeaponType);
                button.onClick.AddListener(() => SelectWeapon(data.WeaponType));
            }
        }

        private void SelectWeapon(WeaponType weaponType)
        {
            ShipModels[OpponentId].SetWeapon(SlotIndex, weaponType);
            HideAsync().Forget();
        }
    }
}
