using Cysharp.Threading.Tasks;
using Infrastructure;
using Ui;
using UI.Battle.Views;
using UI.ShipSetup;
using UnityEngine;
using Zenject;

namespace UI
{
    public sealed class UiFactory : IUIFactory
    {
        private readonly IAssetsInstantiator _instantiator;
        private readonly UIConfig _uiConfig;

        private Transform _rootCanvas;

        [Inject]
        public UiFactory(UIConfig uiConfig, IAssetsInstantiator instantiator)
        {
            _instantiator = instantiator;
            _uiConfig = uiConfig;
        }

        public async UniTask CreateRootAsync()
        {
            if (_rootCanvas == null)
                _rootCanvas = (await _instantiator.CreateAsync(_uiConfig.RootCanvas)).transform;
        }

        public CurtainUIView CreateCurtain()
        {
            var curtainView = _instantiator.Create(_uiConfig.CurtainPrefab);
            curtainView.Init(_uiConfig.CurtainAnimDuration);
            return curtainView;
        }

        public async UniTask<ShipSetupUIView> CreateShipSetupUIAsync()
            => await _instantiator.CreateAsync<ShipSetupUIView>(_uiConfig.ShipSetupMenu, _rootCanvas);

        public async UniTask<BattleUIView> CreateBattleUIAsync()
            => await _instantiator.CreateAsync<BattleUIView>(_uiConfig.BattleUiPrefab, _rootCanvas);

        public async UniTask<ShipSlotUIView> CreateShipEquipmentSlotAsync(Transform parent)
            => await _instantiator.CreateAsync<ShipSlotUIView>(_uiConfig.ShipSlotUiPrefab, parent);

        public async UniTask<SlotUIView> CreateSelectEquipmentSlotAsync(Transform parent)
            => await _instantiator.CreateAsync<SlotUIView>(_uiConfig.SlotUiPrefab, parent);
    }
}