using Battle;
using Cysharp.Threading.Tasks;
using Equipment;
using Ships;
using Ui;
using Zenject;

namespace Infrastructure.GameStates
{
    internal sealed class RunBattleState : IGameState
    {
        private readonly ICurtain _curtain;
        private readonly IWinnerDefiner _winnerDefiner;
        private readonly IUpdater _updater;
        private readonly ICancellationTokenProvider _tokenProvider;
        private readonly IWeaponShotService _weaponShotService;
        private readonly IShipConfigurator _shipConfigurator;

        [Inject]
        public RunBattleState(ICurtain curtain, IWinnerDefiner winnerDefiner, ICancellationTokenProvider tokenProvider, IUpdater updater,
            IWeaponShotService weaponShotService, IShipConfigurator shipConfigurator)
        {
            _curtain = curtain;
            _winnerDefiner = winnerDefiner;
            _updater = updater;
            _tokenProvider = tokenProvider;
            _weaponShotService = weaponShotService;
            _shipConfigurator = shipConfigurator;
        }

        public void Init(IGameStateMachine stateMachine)
        {
        }

        public void Enter()
            => InitAndStartAsync().Forget();

        public void Exit()
        {
            _winnerDefiner.OnWinnerDefined -= HandleBattleStop;
        }

        private async UniTaskVoid InitAndStartAsync()
        {
            using var cts = _tokenProvider.CreateLocalCts();
            _winnerDefiner.OnWinnerDefined += HandleBattleStop;
            await _curtain.SetCurtainVisibleAsync(false, cts.Token);
            StartBattle();
        }

        private void StartBattle()
        {
            foreach (var ship in _shipConfigurator.Ships.Values)
            {
                _updater.AddUpdatable(ship.Health);
                _updater.AddUpdatable(ship.WeaponBattery);
                ship.WeaponBattery.ToggleShooting(true);
            }
        }

        private void HandleBattleStop(IShip winner)
        {
            foreach (var ship in _shipConfigurator.Ships.Values)
            {
                ship.WeaponBattery.ToggleShooting(false);
                _updater.RemoveUpdatable(ship.Health);
                _updater.RemoveUpdatable(ship.WeaponBattery);
            }

            _weaponShotService.DeactivateShotBullets();
        }
    }
}