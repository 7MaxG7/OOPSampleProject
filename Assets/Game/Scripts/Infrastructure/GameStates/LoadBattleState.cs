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
        private readonly IAssetsProvider _assetsProvider;
        private readonly IShipsInitializer _shipsInitializer;
        private readonly IUiFactory _uiFactory;
        private readonly ICancellationTokenProvider _tokenProvider;
        private IGameStateMachine _stateMachine;


        [Inject]
        public LoadBattleState(ISceneLoader sceneLoader, IAssetsProvider assetsProvider, IShipsInitializer shipsInitializer,
            IUiFactory uiFactory, ICancellationTokenProvider tokenProvider)
        {
            _sceneLoader = sceneLoader;
            _assetsProvider = assetsProvider;
            _shipsInitializer = shipsInitializer;
            _uiFactory = uiFactory;
            _tokenProvider = tokenProvider;
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
            _shipsInitializer.CreateShipsAsync();
            await _shipsInitializer.CreateShipsViewsAsync();
        }
    }
}