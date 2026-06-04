using Abstractions.Infrastructure;
using Abstractions.Services;
using Cysharp.Threading.Tasks;
using Utils;
using Zenject;


namespace Infrastructure
{
    internal sealed class LeaveBattleState : IGameState
    {
        private readonly ISceneLoader _sceneLoader;
        private readonly ICancellationTokenProvider _tokenProvider;
        private IGameStateMachine _stateMachine;


        [Inject]
        public LeaveBattleState(ISceneLoader sceneLoader, ICancellationTokenProvider tokenProvider)
        {
            _sceneLoader = sceneLoader;
            _tokenProvider = tokenProvider;
        }

        public void Enter()
            => LoadShipSetupAsync().Forget();

        public void Exit()
        {
        }

        public void Init(IGameStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        private async UniTaskVoid LoadShipSetupAsync()
        {
            using var cts = _tokenProvider.CreateLocalCts();
            await _sceneLoader.LoadSceneAsync(Constants.SETUP_SCENE_NAME, cts);
            _stateMachine.Enter<ShipSetupState>();
        }
    }
}