using System;
using Battle;
using Cysharp.Threading.Tasks;
using Infrastructure;
using Ships;
using Ui;
using Zenject;

namespace UI.Battle
{
    public class BattleUIBuilder : IBattleUIBuilder
    {
        private readonly IUiFactory _uiFactory;
        private readonly IShipConfigurator _shipConfigurator;
        private readonly IWinnerDefiner _winnerDefiner;
        private readonly BattleUIModel _battleUIModel;
        private readonly ICancellationTokenProvider _tokenProvider;

        private BattleUIController _battleUiController;
        private bool _isInited;

        [Inject]
        public BattleUIBuilder(IUiFactory uiFactory, IShipConfigurator shipConfigurator, IWinnerDefiner winnerDefiner, ICleaner cleaner,
            BattleUIModel battleUIModel, ICancellationTokenProvider tokenProvider)
        {
            _uiFactory = uiFactory;
            _shipConfigurator = shipConfigurator;
            _winnerDefiner = winnerDefiner;
            _battleUIModel = battleUIModel;
            _tokenProvider = tokenProvider;

            cleaner.AddCleanable(this);
        }

        public void CleanUp()
        {
            if (!_isInited)
                return;

            foreach (var (opponentId, healthModel) in _battleUIModel.ShipHealthModels)
            {
                if (!_shipConfigurator.TryGetShip(opponentId, out var ship))
                    continue;

                ship.Health.OnHpChanged -= healthModel.SetHp;
                ship.Health.OnShieldChanged -= healthModel.SetShield;
            }

            _battleUiController.Clean();
            _battleUIModel.ShipHealthModels.Clear();

            _isInited = false;
        }

        public async UniTask BuildUI(Action leaveBattle)
        {
            await _uiFactory.CreateRootAsync();
            _battleUiController = new BattleUIController(_tokenProvider, _winnerDefiner);
            var view = await _uiFactory.CreateBattleUIAsync();
            var model = SetupModel();
            _battleUiController.Init(model, view, leaveBattle);

            _isInited = true;
        }

        private BattleUIModel SetupModel()
        {
            foreach (var (opponentId, ship) in _shipConfigurator.Ships)
                _battleUIModel.ShipHealthModels.Add(opponentId, CreateHealthPanelModel(ship));
            return _battleUIModel;
        }

        private BattleShipHealthUIModel CreateHealthPanelModel(IShip ship)
        {
            var model = new BattleShipHealthUIModel();

            ship.Health.OnHpChanged += model.SetHp;
            ship.Health.OnShieldChanged += model.SetShield;
            model.SetHp(ship.Health.CurrentHp, ship.Health.MaxHp);
            model.SetShield(ship.Health.CurrentShield, ship.Health.MaxShield);

            return model;
        }
    }
}