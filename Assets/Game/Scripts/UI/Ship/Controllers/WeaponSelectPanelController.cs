using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Equipment;
using Equipment.Data;
using Infrastructure;
using Ships.Data;
using UI.Ship.Models;
using UI.Ship.Views;

namespace UI.Ship
{
    public sealed class WeaponSelectPanelController : AbstractEquipmentSelectController<WeaponType>
    {
        public WeaponSelectPanelController(WeaponSelectView weaponSelectView, Dictionary<OpponentId, ShipModel> shipModels,
            ICancellationTokenProvider tokenProvider) : base(weaponSelectView, shipModels, tokenProvider) { }

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
            ShipModels[OpponentId].SetWeapon(SlotIndex, weaponType);
            HideAsync().Forget();
        }
    }
}
