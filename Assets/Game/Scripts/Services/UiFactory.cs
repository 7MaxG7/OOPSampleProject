using Cysharp.Threading.Tasks;
using Configs;
using Enums;
using Infrastructure;
using UI;
using UI.Battle;
using UI.Battle.Views;
using UI.ShipSetup;
using UnityEngine;
using Zenject;

namespace Services
{
    public sealed class UiFactory : IUiFactory
    {
        private readonly IAssetsProvider _assetsProvider;
        private readonly IStaticDataService _staticDataService;
        private readonly IShipConfigurationsHolder _configurationsHolder;
        private readonly ICancellationTokenProvider _tokenProvider;
        private readonly UiConfig _uiConfig;
        
        private Transform _rootCanvas;


        [Inject]
        public UiFactory(IAssetsProvider assetsProvider, IStaticDataService staticDataService
            , IShipConfigurationsHolder configurationsHolder, UiConfig uiConfig, ICancellationTokenProvider tokenProvider)
        {
            _assetsProvider = assetsProvider;
            _staticDataService = staticDataService;
            _configurationsHolder = configurationsHolder;
            _uiConfig = uiConfig;
            _tokenProvider = tokenProvider;
        }
        
        public async UniTask PrepareCanvasAsync()
        {
            if (_rootCanvas == null)
                _rootCanvas = (await _assetsProvider.CreateInstanceAsync(_uiConfig.RootCanvas)).transform;
        }

        public async UniTask<CurtainView> CreateCurtainAsync()
        {
            var curtainView = await _assetsProvider.CreateInstanceAsync<CurtainView>(_uiConfig.CurtainPrefab, isDontDestroyAsset: true);
            curtainView.Init(_uiConfig.CurtainAnimDuration);
            return curtainView;
        }

        public async UniTask<ShipSetupMenuController> CreateShipSetupMenuAsync()
        {
            var view = await _assetsProvider.CreateInstanceAsync<ShipSetupMenuView>(_uiConfig.ShipSetupMenu, _rootCanvas);
            return new ShipSetupMenuController(view, _configurationsHolder.ShipModels, _tokenProvider);
        }

        public async UniTask<BattleUiController> CreateBattleUiAsync()
        {
            var view = await _assetsProvider.CreateInstanceAsync<BattleUiView>(_uiConfig.BattleUiPrefab, _rootCanvas);
            return new BattleUiController(view);
        }

        public async UniTask<SlotUiView> CreateSelectWeaponUiSlotAsync(WeaponType weaponType, Transform parent)
        {
            var slot = await CreateSelectEquipmentUiSlotAsync(parent);
            slot.SetIcon(_staticDataService.GetWeaponData(weaponType).Icon);
            return slot;
        }

        public async UniTask<SlotUiView> CreateSelectModuleUiSlotAsync(ModuleType moduleType, Transform parent)
        {
            var slot = await CreateSelectEquipmentUiSlotAsync(parent);
            slot.SetIcon(_staticDataService.GetModuleData(moduleType).Icon);
            return slot;
        }

        public async UniTask<ShipSlotUiView> CreateEquipmentUiSlotAsync(Transform parent) 
            => await _assetsProvider.CreateInstanceAsync<ShipSlotUiView>(_uiConfig.ShipSlotUiPrefab, parent);

        private async UniTask<SlotUiView> CreateSelectEquipmentUiSlotAsync(Transform parent) 
            => await _assetsProvider.CreateInstanceAsync<SlotUiView>(_uiConfig.SlotUiPrefab, parent);
    }
}
