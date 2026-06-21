using Cysharp.Threading.Tasks;
using Ships;
using Sounds;
using Ui;
using UI.Ship;
using Zenject;

namespace Infrastructure.GameStates
{
    internal sealed class ShipSetupState : IGameState
    {
        private readonly ICurtain _curtain;
        private readonly IShipsInitializer _shipsInitializer;
        private readonly ISoundService _soundService;
        private readonly IAssetsProvider _assetsProvider;
        private readonly IShipSetupUIService _shipSetupUIService;
        private readonly IUiFactory _uiFactory;
        private readonly ICleaner _cleaner;
        private readonly ICancellationTokenProvider _tokenProvider;

        private IGameStateMachine _stateMachine;
        private ShipSetupController _shipSetup;

        [Inject]
        public ShipSetupState(IShipsInitializer shipsInitializer, ISoundService soundService, ICancellationTokenProvider tokenProvider,
            ICurtain curtain, IAssetsProvider assetsProvider, IShipSetupUIService shipSetupUIService, IUiFactory uiFactory,
            ICleaner cleaner)
        {
            _curtain = curtain;
            _shipsInitializer = shipsInitializer;
            _soundService = soundService;
            _assetsProvider = assetsProvider;
            _shipSetupUIService = shipSetupUIService;
            _uiFactory = uiFactory;
            _cleaner = cleaner;
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
            _shipSetup.OnSetupComplete -= SwitchState;
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
            _shipsInitializer.CreateShipsAsync();
            await _shipsInitializer.CreateShipsViewsAsync();
            await SetupUiAsync();
        }

        private async UniTask SetupUiAsync()
        {
            await _uiFactory.CreateRootAsync();
            _shipSetup = await _uiFactory.CreateShipSetupUIAsync();
            await _shipSetup.SetupUiAsync();
            _shipSetup.OnSetupComplete += SwitchState;
            _cleaner.AddCleanable(_shipSetup);
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