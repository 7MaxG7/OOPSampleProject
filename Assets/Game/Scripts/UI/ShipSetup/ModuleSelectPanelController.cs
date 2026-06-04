using System.Collections.Generic;
using Configs.Data;
using Cysharp.Threading.Tasks;
using Enums;
using Infrastructure;
using Ships;

namespace UI.ShipSetup
{
    public sealed class ModuleSelectPanelController : AbstractEquipmentSelectController<ModuleType>
    {
        public ModuleSelectPanelController(ModuleSelectView moduleSelectView, Dictionary<OpponentId, ShipModel> shipModels,
            ICancellationTokenProvider tokenProvider) : base(moduleSelectView, shipModels, tokenProvider) { }

        public async UniTask SetupModuledSelectPanelAsync(ModuleData[] moduleDatas)
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
