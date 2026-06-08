using Battle;
using Cysharp.Threading.Tasks;
using Equipment;
using Ships;
using Ui;
using Utils;
using Zenject;

namespace Infrastructure.GameStates
{
    internal sealed class LoadBattleState : IGameState
    {
        private readonly ISceneLoader _sceneLoader;
        private readonly IWinnerDefiner _winnerDefiner;
        private readonly IAssetsProvider _assetsProvider;
        private readonly IShipsInitializer _shipsInitializer;
        private readonly IAmmoFactory _ammoFactory;
        private readonly IUiFactory _uiFactory;
        private readonly IDamageHandler _damageHandler;
        private readonly ICancellationTokenProvider _tokenProvider;
        private IGameStateMachine _stateMachine;


        [Inject]
        public LoadBattleState(ISceneLoader sceneLoader, IWinnerDefiner winnerDefiner, IAssetsProvider assetsProvider
            , IShipsInitializer shipsInitializer, IAmmoFactory ammoFactory, IUiFactory uiFactory, IDamageHandler damageHandler,
            ICancellationTokenProvider tokenProvider)
        {
            _sceneLoader = sceneLoader;
            _winnerDefiner = winnerDefiner;
            _assetsProvider = assetsProvider;
            _shipsInitializer = shipsInitializer;
            _ammoFactory = ammoFactory;
            _uiFactory = uiFactory;
            _damageHandler = damageHandler;
            _tokenProvider = tokenProvider;
        }

        public void Enter()
            => LoadBattleAsync().Forget();

        private async UniTaskVoid LoadBattleAsync()
        {
            using var cts = _tokenProvider.CreateLocalCts();
            await _sceneLoader.LoadSceneAsync(Constants.BATTLE_SCENE_NAME, cts);

            await _assetsProvider.WarmUpCurrentSceneAsync();
            await _uiFactory.CreateRootAsync();
            await CreateOpponentsAsync();

            _stateMachine.Enter<RunBattleState>();
        }

        public void Exit()
        {
        }

        public void Init(IGameStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        private async UniTask CreateOpponentsAsync()
        {
            await _shipsInitializer.CreateShipsAsync();

            foreach (var ship in _shipsInitializer.Ships.Values)
            {
                ship.PrepareToBattle();
                _winnerDefiner.AddShip(ship);
                _damageHandler.AddShip(ship);
            }
        }
    }
}