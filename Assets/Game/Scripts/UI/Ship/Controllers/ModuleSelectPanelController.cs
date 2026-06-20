using Cysharp.Threading.Tasks;
using Equipment;
using Infrastructure;
using Ships;
using Ui;

namespace UI.Ship
{
    public sealed class ModuleSelectPanelController : BaseEquipmentSelectController
    {
        private readonly IShipSetupUIService _shipSetupUIService;
        private readonly IStaticDataService _staticDataService;

        public ModuleSelectPanelController(IShipConfigurator shipConfigurator, IUiFactory uiFactory, IShipSetupUIService shipSetupUIService
            , ICancellationTokenProvider tokenProvider, IStaticDataService staticDataService, UiConfig uiConfig) : base(shipConfigurator
            , tokenProvider, uiFactory, uiConfig)
        {
            _shipSetupUIService = shipSetupUIService;
            _staticDataService = staticDataService;
        }

        protected override async UniTask SetupEquipSelectPanelAsync()
        {
            foreach (var config in _staticDataService.GetAllEnabledModules())
            {
                var slot = await CreateEquipmentSelectSlotAsync();
                slot.SetIcon(_shipSetupUIService.GetModuleIcon(config.ModuleType));
                slot.SelectButton.onClick.AddListener(() => SelectModule(config.ModuleType));
            }
        }

        private void SelectModule(ModuleType moduleType)
        {
            ShipConfigurator.SetModule(OpponentId, SlotIndex, moduleType);
            HideAsync().Forget();
        }
    }
}