using Cysharp.Threading.Tasks;
using Equipment;
using Infrastructure;
using Ships;
using Ui;

namespace UI.Ship
{
    public sealed class WeaponSelectPanelController : BaseEquipmentSelectController
    {
        private readonly IShipSetupUIService _shipSetupUIService;
        private readonly IStaticDataService _staticDataService;

        public WeaponSelectPanelController(IShipConfigurator shipConfigurator, IUiFactory uiFactory, IShipSetupUIService shipSetupUIService,
            ICancellationTokenProvider tokenProvider, IStaticDataService staticDataService, UiConfig uiConfig) : base(shipConfigurator,
            tokenProvider, uiFactory, uiConfig)
        {
            _shipSetupUIService = shipSetupUIService;
            _staticDataService = staticDataService;
        }

        protected override async UniTask SetupEquipSelectPanelAsync()
        {
            foreach (var config in _staticDataService.GetAllEnabledWeapons())
            {
                var slot = await CreateEquipmentSelectSlotAsync();
                slot.SetIcon(_shipSetupUIService.GetWeaponIcon(config.WeaponType));
                slot.SelectButton.onClick.AddListener(() => SelectWeapon(config.WeaponType));
            }
        }

        private void SelectWeapon(WeaponType weaponType)
        {
            ShipConfigurator.SetWeapon(OpponentId, SlotIndex, weaponType);
            HideAsync().Forget();
        }
    }
}