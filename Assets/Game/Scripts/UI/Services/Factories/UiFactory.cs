using Cysharp.Threading.Tasks;
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
        private readonly IShipSetupUIService _shipSetupUIService;
        private readonly ICleaner _cleaner;

        private Transform _rootCanvas;

        [Inject]
        public UiFactory(IStaticDataService staticDataService, IShipConfigurator shipConfigurator, UiConfig uiConfig, ICleaner cleaner,
            IAssetsInstantiator instantiator, ICancellationTokenProvider tokenProvider, IShipSetupUIService shipSetupUIService)
        {
            _staticDataService = staticDataService;
            _instantiator = instantiator;
            _shipConfigurator = shipConfigurator;
            _uiConfig = uiConfig;
            _tokenProvider = tokenProvider;
            _shipSetupUIService = shipSetupUIService;
            _cleaner = cleaner;
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

        public async UniTask<ShipSetupController> CreateShipSetupUIAsync()
        {
            var view = await _instantiator.CreateAsync<ShipSetupView>(_uiConfig.ShipSetupMenu, _rootCanvas);
            return new ShipSetupController(view, _shipConfigurator, _tokenProvider, _staticDataService, this, _uiConfig,
                _shipSetupUIService, _cleaner);
        }

        public async UniTask<BattleUiController> CreateBattleUIAsync()
        {
            var view = await _instantiator.CreateAsync<BattleUiView>(_uiConfig.BattleUiPrefab, _rootCanvas);
            return new BattleUiController(view);
        }

        public async UniTask<ShipSlotUiView> CreateShipEquipmentSlotAsync(Transform parent)
            => await _instantiator.CreateAsync<ShipSlotUiView>(_uiConfig.ShipSlotUiPrefab, parent);

        public async UniTask<SlotUiView> CreateSelectEquipmentSlotAsync(Transform parent)
            => await _instantiator.CreateAsync<SlotUiView>(_uiConfig.SlotUiPrefab, parent);
    }
}