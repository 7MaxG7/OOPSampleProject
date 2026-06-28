using Cysharp.Threading.Tasks;
using Infrastructure;
using Ui;
using UI.Battle.Views;
using UI.ShipSetup;
using UnityEngine;
using Zenject;

namespace UI
{
    public sealed class UiFactory : IUiFactory
    {
        private readonly IAssetsInstantiator _instantiator;
        private readonly UiConfig _uiConfig;

        private Transform _rootCanvas;

        [Inject]
        public UiFactory(UiConfig uiConfig, IAssetsInstantiator instantiator)
        {
            _instantiator = instantiator;
            _uiConfig = uiConfig;
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

        public async UniTask<ShipSetupView> CreateShipSetupUIAsync()
            => await _instantiator.CreateAsync<ShipSetupView>(_uiConfig.ShipSetupMenu, _rootCanvas);

        public async UniTask<BattleUiView> CreateBattleUIAsync()
            => await _instantiator.CreateAsync<BattleUiView>(_uiConfig.BattleUiPrefab, _rootCanvas);

        public async UniTask<ShipSlotUiView> CreateShipEquipmentSlotAsync(Transform parent)
            => await _instantiator.CreateAsync<ShipSlotUiView>(_uiConfig.ShipSlotUiPrefab, parent);

        public async UniTask<SlotUiView> CreateSelectEquipmentSlotAsync(Transform parent)
            => await _instantiator.CreateAsync<SlotUiView>(_uiConfig.SlotUiPrefab, parent);
    }
}