using Cysharp.Threading.Tasks;
using Ships;
using Sounds;
using Ui;
using Zenject;

namespace Infrastructure.GameStates
{
    internal sealed class ShipSetupState : IGameState
    {
        private readonly ICurtain _curtain;
        private readonly IShipsInitializer _shipsInitializer;
        private readonly IShipsViewInitializer _shipsViewInitializer;
        private readonly ISoundService _soundService;
        private readonly IAssetsProvider _assetsProvider;
        private readonly IShipSetupUIService _shipSetupUIService;
        private readonly IShipSetupUIBuilder _shipSetupUIBuilder;
        private readonly ICancellationTokenProvider _tokenProvider;

        private IGameStateMachine _stateMachine;

        [Inject]
        public ShipSetupState(IShipsInitializer shipsInitializer, IShipsViewInitializer shipsViewInitializer,
            ISoundService soundService, ICancellationTokenProvider tokenProvider, ICurtain curtain, IAssetsProvider assetsProvider,
            IShipSetupUIService shipSetupUIService, IShipSetupUIBuilder shipSetupUIBuilder)
        {
            _curtain = curtain;
            _shipsInitializer = shipsInitializer;
            _shipsViewInitializer = shipsViewInitializer;
            _soundService = soundService;
            _assetsProvider = assetsProvider;
            _shipSetupUIService = shipSetupUIService;
            _shipSetupUIBuilder = shipSetupUIBuilder;
            _tokenProvider = tokenProvider;
        }

        public void Init(IGameStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void Enter()
            => StartSetupAsync().Forget();

        public void Exit()
        {
        }

        private async UniTaskVoid StartSetupAsync()
        {
            using var cts = _tokenProvider.CreateLocalCts();

            await _assetsProvider.WarmUpCurrentSceneAsync();
            _shipSetupUIService.Init();
            await InitSceneAsync();

            _soundService.PlayMusic();
            await _curtain.SetCurtainVisibleAsync(false, cts.Token);
        }

        private async UniTask InitSceneAsync()
        {
            _shipsInitializer.CreateShips();
            await _shipsViewInitializer.CreateShipsViewsAsync();
            await _shipSetupUIBuilder.BuildUIAsync(SwitchState);
        }

        private void SwitchState()
            => SwitchStateAsync().Forget();

        private async UniTaskVoid SwitchStateAsync()
        {
            using var cts = _tokenProvider.CreateLocalCts();
            await _curtain.SetCurtainVisibleAsync(true, cts.Token);
            _stateMachine.Enter<LoadBattleState>();
        }
    }
}