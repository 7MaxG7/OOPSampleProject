using Cysharp.Threading.Tasks;
using Equipment;
using Equipment.Data;
using Infrastructure;
using Ships;

namespace UI.Ship
{
    public sealed class ModuleSelectPanelController : BaseEquipmentSelectController<ModuleType>
    {
        public ModuleSelectPanelController(ModuleSelectView moduleSelectView, IShipConfigurator shipConfigurator,
            ICancellationTokenProvider tokenProvider) : base(moduleSelectView, shipConfigurator, tokenProvider) { }

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
            ShipConfigurator.SetModule(OpponentId, SlotIndex, moduleType);
            HideAsync().Forget();
        }
    }
}
