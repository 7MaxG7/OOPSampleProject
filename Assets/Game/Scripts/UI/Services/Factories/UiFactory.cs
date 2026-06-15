using Cysharp.Threading.Tasks;
using Equipment.Data;
using Infrastructure;
using Ships;
using Ui;
using UI.Battle;
using UI.Battle.Views;
using UI.Ship;
using UnityEngine;
using Zenject;

namespace UI
{
    public sealed class UiFactory : IUiFactory
    {
        private readonly IStaticDataService _staticDataService;
        private readonly IAssetsInstantiator _instantiator;
        private readonly IShipConfigurator _shipConfigurator;
        private readonly ICancellationTokenProvider _tokenProvider;
        private readonly UiConfig _uiConfig;

        private Transform _rootCanvas;

        [Inject]
        public UiFactory(IStaticDataService staticDataService, IShipConfigurator shipConfigurator, UiConfig uiConfig,
            IAssetsInstantiator instantiator, ICancellationTokenProvider tokenProvider)
        {
            _staticDataService = staticDataService;
            _instantiator = instantiator;
            _shipConfigurator = shipConfigurator;
            _uiConfig = uiConfig;
            _tokenProvider = tokenProvider;
        }

        public async UniTask CreateRootAsync()
        {
            if (_rootCanvas == null)
                _rootCanvas = (await _instantiator.CreateAsync(_uiConfig.RootCanvas)).transform;
        }

        public CurtainView CreateCurtain()
        {
            var curtainView = _instantiator.Create(_uiConfig.CurtainPrefab);
            curtainView.Init(_uiConfig.CurtainAnimDuration);
            return curtainView;
        }

        public async UniTask<ShipSetupMenuController> CreateShipSetupMenuAsync()
        {
            var view = await _instantiator.CreateAsync<ShipSetupMenuView>(_uiConfig.ShipSetupMenu, _rootCanvas);
            return new ShipSetupMenuController(view, _shipConfigurator, _tokenProvider);
        }

        public async UniTask<BattleUiController> CreateBattleUiAsync()
        {
            var view = await _instantiator.CreateAsync<BattleUiView>(_uiConfig.BattleUiPrefab, _rootCanvas);
            return new BattleUiController(view);
        }

        public async UniTask<SlotUiView> CreateSelectWeaponUiSlotAsync(WeaponType weaponType, Transform parent)
        {
            var slot = await CreateSelectEquipmentUiSlotAsync(parent);
            slot.SetIcon(_staticDataService.GetWeapon(weaponType).Icon);
            return slot;
        }

        public async UniTask<SlotUiView> CreateSelectModuleUiSlotAsync(ModuleType moduleType, Transform parent)
        {
            var slot = await CreateSelectEquipmentUiSlotAsync(parent);
            slot.SetIcon(_staticDataService.GetModule(moduleType).Icon);
            return slot;
        }

        public async UniTask<ShipSlotUiView> CreateEquipmentUiSlotAsync(Transform parent)
            => await _instantiator.CreateAsync<ShipSlotUiView>(_uiConfig.ShipSlotUiPrefab, parent);

        private async UniTask<SlotUiView> CreateSelectEquipmentUiSlotAsync(Transform parent)
            => await _instantiator.CreateAsync<SlotUiView>(_uiConfig.SlotUiPrefab, parent);
    }
}