using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Equipment;
using Equipment.Data;
using Infrastructure;
using Ships;
using Ships.Data;
using UI.Ship.Models;
using UI.Ship.Views;

namespace UI.Ship
{
    public sealed class ModuleSelectPanelController : AbstractEquipmentSelectController<ModuleType>
    {
        public ModuleSelectPanelController(ModuleSelectView moduleSelectView, Dictionary<OpponentId, ShipModel> shipModels,
            ICancellationTokenProvider tokenProvider) : base(moduleSelectView, shipModels, tokenProvider) { }

        public async UniTask SetupModuledSelectPanelAsync(ModuleConfig[] moduleDatas)
        {
            foreach (var data in moduleDatas)
            {
                var button = await EquipmentSelectView.AddEquipmentSelectSlot(data.ModuleType);
                button.onClick.AddListener(() => SelectModule(data.ModuleType));
            }
        }

        private void SelectModule(ModuleType moduleType)
        {
            ShipModels[OpponentId].SetModule(SlotIndex, moduleType);
            HideAsync().Forget();
        }
    }
}
