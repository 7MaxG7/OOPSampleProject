using System.Collections.Generic;
using Abstractions.Infrastructure;
using Cysharp.Threading.Tasks;
using Configs.Data;
using Enums;
using Ships;

namespace Ui.ShipSetup
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
