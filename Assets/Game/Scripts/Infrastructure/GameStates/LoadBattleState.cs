using Cysharp.Threading.Tasks;
using Services;
using Ships;
using Utils;
using Zenject;


namespace Infrastructure
{
    internal sealed class LoadBattleState : IGameState
    {
        private readonly ISceneLoader _sceneLoader;
        private readonly IBattleObserver _battleObserver;
        private readonly IAssetsProvider _assetsProvider;
        private readonly IShipsInitializer _shipsInitializer;
        private readonly IAmmoFactory _ammoFactory;
        private readonly IUiFactory _uiFactory;
        private readonly IDamageHandler _damageHandler;
        private readonly ICancellationTokenProvider _tokenProvider;
        private IGameStateMachine _stateMachine;


        [Inject]
        public LoadBattleState(ISceneLoader sceneLoader, IBattleObserver battleObserver, IAssetsProvider assetsProvider
            , IShipsInitializer shipsInitializer, IAmmoFactory ammoFactory, IUiFactory uiFactory, IDamageHandler damageHandler,
            ICancellationTokenProvider tokenProvider)
        {
            _sceneLoader = sceneLoader;
            _battleObserver = battleObserver;
            _assetsProvider = assetsProvider;
            _shipsInitializer = shipsInitializer;
            _ammoFactory = ammoFactory;
            _uiFactory = uiFactory;
            _damageHandler = damageHandler;
            _tokenProvider = tokenProvider;
        }

        public void Enter()
        {
            InitAndRunBattleAsync().Forget();
        }

        private async UniTaskVoid InitAndRunBattleAsync()
        {
            using var cts = _tokenProvider.CreateLocalCts();
            await _sceneLoader.LoadSceneAsync(Constants.BATTLE_SCENE_NAME, cts);

            await _assetsProvider.WarmUpCurrentSceneAsync();
            await _uiFactory.PrepareCanvasAsync();
            _ammoFactory.PrepareRoot();
            PrepareOpponents();

            _stateMachine.Enter<RunBattleState>();
        }

        public void Exit()
        {
        }

        public void Init(IGameStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        private void PrepareOpponents()
        {
            _shipsInitializer.PrepareShipsAsync();

            foreach (var ship in _shipsInitializer.Ships.Values)
            {
                ship.PrepareToBattle();
                _battleObserver.AddShip(ship);
                _damageHandler.AddShip(ship);
            }
        }
    }
}