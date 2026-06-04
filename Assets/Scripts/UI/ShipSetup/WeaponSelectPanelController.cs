using System.Collections.Generic;
using Abstractions.Infrastructure;
using Cysharp.Threading.Tasks;
using Configs.Data;
using Enums;
using Ships;

namespace Ui.ShipSetup
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
