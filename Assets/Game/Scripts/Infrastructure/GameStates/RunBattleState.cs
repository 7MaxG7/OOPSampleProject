using Battle;
using Cysharp.Threading.Tasks;
using Infrastructure.ControllersHolder;
using Ships;
using Ui;
using UI.Battle;
using Zenject;

namespace Infrastructure.GameStates
{
    internal sealed class RunBattleState : IGameState
    {
        private readonly ICurtain _curtain;
        private readonly IBattleObserver _battleObserver;
        private readonly IShipsInitializer _shipsInitializer;
        private readonly IUpdater _updater;
        private readonly IUiFactory _uiFactory;
        private readonly ICancellationTokenProvider _tokenProvider;
        private IGameStateMachine _stateMachine;
        
        private BattleUiController _battleUi;


        [Inject]
        public RunBattleState(ICurtain curtain, IBattleObserver battleObserver, IShipsInitializer shipsInitializer
            , IUpdater updater, IUiFactory uiFactory, ICancellationTokenProvider tokenProvider)
        {
            _curtain = curtain;
            _battleObserver = battleObserver;
            _shipsInitializer = shipsInitializer;
            _updater = updater;
            _uiFactory = uiFactory;
            _tokenProvider = tokenProvider;
        }
        
        public void Enter()
        {
            InitAndStartAsync().Forget();
        }

        private async UniTaskVoid InitAndStartAsync()
        {
            using var cts = _tokenProvider.CreateLocalCts();
            await SetupUIAsync();
            _battleObserver.OnWinnerDefined += HandleBattleStop;
            await _curtain.SetCurtainVisibleAsync(false, cts.Token);
            StartBattle();
        }

        public void Exit()
        {
            _battleUi.CleanUp();
            _battleObserver.OnWinnerDefined -= HandleBattleStop;
            _battleUi.OnBattleLeft -= LeaveBattle;
        }

        public void Init(IGameStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        private async UniTask SetupUIAsync()
        {
            _battleUi = await _uiFactory.CreateBattleUiAsync();
            _battleUi.SetupUi(_shipsInitializer.Ships);
            _battleUi.OnBattleLeft += LeaveBattle;
        }

        private void StartBattle()
        {
            foreach (var ship in _battleObserver.Ships)
            {
                _updater.AddUpdatable(ship.Health);
                _updater.AddUpdatable(ship.WeaponBattery);
                ship.WeaponBattery.ToggleShooting(true);
            }
        }

        private void HandleBattleStop(IShip winner)
        {
            foreach (var ship in _battleObserver.Ships)
            {
                ship.WeaponBattery.ToggleShooting(false);
                _updater.RemoveController(ship.Health);
                _updater.RemoveController(ship.WeaponBattery);
            }

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