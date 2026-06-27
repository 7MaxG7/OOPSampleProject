using Battle;
using Cysharp.Threading.Tasks;
using Equipment;
using Ships;
using Ui;
using UI.Battle;
using Zenject;

namespace Infrastructure.GameStates
{
    internal sealed class RunBattleState : IGameState
    {
        private readonly ICurtain _curtain;
        private readonly IWinnerDefiner _winnerDefiner;
        private readonly IUpdater _updater;
        private readonly IUiFactory _uiFactory;
        private readonly ICancellationTokenProvider _tokenProvider;
        private readonly IShipConfigurator _shipConfigurator;
        private readonly IWeaponShotService _weaponShotService;
        private IGameStateMachine _stateMachine;

        private BattleUiController _battleUi;

        [Inject]
        public RunBattleState(ICurtain curtain, IWinnerDefiner winnerDefiner, IShipsInitializer shipsInitializer, IUiFactory uiFactory,
            ICancellationTokenProvider tokenProvider, IShipConfigurator shipConfigurator, IWeaponShotService weaponShotService,
            IUpdater updater)
        {
            _curtain = curtain;
            _winnerDefiner = winnerDefiner;
            _updater = updater;
            _uiFactory = uiFactory;
            _tokenProvider = tokenProvider;
            _shipConfigurator = shipConfigurator;
            _weaponShotService = weaponShotService;
        }

        public void Init(IGameStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void Enter()
            => InitAndStartAsync().Forget();

        public void Exit()
        {
            _battleUi.CleanUp();
            _winnerDefiner.OnWinnerDefined -= HandleBattleStop;
            _battleUi.OnBattleLeft -= LeaveBattle;
        }

        private async UniTaskVoid InitAndStartAsync()
        {
            using var cts = _tokenProvider.CreateLocalCts();
            await SetupUIAsync();
            _winnerDefiner.OnWinnerDefined += HandleBattleStop;
            await _curtain.SetCurtainVisibleAsync(false, cts.Token);
            StartBattle();
        }

        private async UniTask SetupUIAsync()
        {
            _battleUi = await _uiFactory.CreateBattleUIAsync();
            _battleUi.SetupUi(_shipConfigurator.Ships);
            _battleUi.OnBattleLeft += LeaveBattle;
        }

        private void StartBattle()
        {
            foreach (var ship in _winnerDefiner.Ships)
            {
                _updater.AddUpdatable(ship.Health);
                _updater.AddUpdatable(ship.WeaponBattery);
                ship.WeaponBattery.ToggleShooting(true);
            }
        }

        private void HandleBattleStop(IShip winner)
        {
            foreach (var ship in _winnerDefiner.Ships)
            {
                ship.WeaponBattery.ToggleShooting(false);
                _updater.RemoveUpdatable(ship.Health);
                _updater.RemoveUpdatable(ship.WeaponBattery);
            }

            _weaponShotService.DeactivateShotBullets();

            _battleUi.ShowBattleEnd(winner);
        }

        private void LeaveBattle()
            => LeaveBattleAsync().Forget();

        private async UniTaskVoid LeaveBattleAsync()
        {
            using var cts = _tokenProvider.CreateLocalCts();
            await _curtain.SetCurtainVisibleAsync(true, cts.Token);
            _stateMachine.Enter<LeaveBattleState>();
        }
    }
}
