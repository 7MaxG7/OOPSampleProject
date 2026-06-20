using Battle;
using Cysharp.Threading.Tasks;
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
        private readonly IUiFactory _uiFactory;
        private readonly IDamageHandler _damageHandler;
        private readonly ICancellationTokenProvider _tokenProvider;
        private readonly IShipConfigurator _shipConfigurator;
        private IGameStateMachine _stateMachine;


        [Inject]
        public LoadBattleState(ISceneLoader sceneLoader, IWinnerDefiner winnerDefiner, IAssetsProvider assetsProvider
            , IShipsInitializer shipsInitializer, IUiFactory uiFactory, IDamageHandler damageHandler, IShipConfigurator shipConfigurator,
            ICancellationTokenProvider tokenProvider)
        {
            _sceneLoader = sceneLoader;
            _winnerDefiner = winnerDefiner;
            _assetsProvider = assetsProvider;
            _shipsInitializer = shipsInitializer;
            _uiFactory = uiFactory;
            _damageHandler = damageHandler;
            _tokenProvider = tokenProvider;
            _shipConfigurator = shipConfigurator;
        }

        public void Init(IGameStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
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

        private async UniTask CreateOpponentsAsync()
        {
            await _shipsInitializer.CreateShipsAsync();

            foreach (var ship in _shipConfigurator.Ships.Values)
            {
                ship.PrepareToBattle();
                _winnerDefiner.AddShip(ship);
                _damageHandler.AddShip(ship);
            }
        }
    }
}