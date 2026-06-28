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
        private readonly IShipsViewInitializer _shipsViewInitializer;
        private readonly ICurtain _curtain;
        private readonly ICancellationTokenProvider _tokenProvider;
        private readonly IBattleUIBuilder _battleUIBuilder;
        private IGameStateMachine _stateMachine;

        [Inject]
        public LoadBattleState(ISceneLoader sceneLoader, IAssetsProvider assetsProvider, IShipsInitializer shipsInitializer,
            IShipsViewInitializer shipsViewInitializer, ICancellationTokenProvider tokenProvider, IBattleUIBuilder battleUIBuilder,
            ICurtain curtain)
        {
            _sceneLoader = sceneLoader;
            _assetsProvider = assetsProvider;
            _shipsInitializer = shipsInitializer;
            _shipsViewInitializer = shipsViewInitializer;
            _curtain = curtain;
            _tokenProvider = tokenProvider;
            _battleUIBuilder = battleUIBuilder;
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
            _shipsInitializer.CreateShips();
            await _shipsViewInitializer.CreateShipsViewsAsync();
            await _battleUIBuilder.BuildUI(LeaveBattle);

            _stateMachine.Enter<RunBattleState>();
        }

        public void Exit()
        {
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