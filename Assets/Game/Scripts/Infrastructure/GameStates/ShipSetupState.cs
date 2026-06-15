using Cysharp.Threading.Tasks;
using Ships;
using Sounds;
using Ui;
using UI;
using UI.Ship;
using Zenject;

namespace Infrastructure.GameStates
{
    internal sealed class ShipSetupState : IGameState
    {
        private readonly ICurtain _curtain;
        private readonly IShipsInitializer _shipsInitializer;
        private readonly ISoundService _soundService;
        private readonly IStaticDataService _staticDataService;
        private readonly IAssetsProvider _assetsProvider;
        private readonly IUiFactory _uiFactory;
        private readonly ICleaner _cleaner;
        private readonly UiConfig _uiConfig;
        private readonly ICancellationTokenProvider _tokenProvider;

        private IGameStateMachine _stateMachine;
        private ShipSetupMenuController _shipSetupMenu;

        [Inject]
        public ShipSetupState(ICurtain curtain, IShipsInitializer shipsInitializer, IStaticDataService staticDataService
            , ISoundService soundService, IAssetsProvider assetsProvider, IUiFactory uiFactory, ICleaner cleaner, UiConfig uiConfig,
            ICancellationTokenProvider tokenProvider)
        {
            _curtain = curtain;
            _shipsInitializer = shipsInitializer;
            _soundService = soundService;
            _staticDataService = staticDataService;
            _assetsProvider = assetsProvider;
            _uiFactory = uiFactory;
            _cleaner = cleaner;
            _uiConfig = uiConfig;
            _tokenProvider = tokenProvider;
        }

        public void Enter()
            => StartSetupAsync().Forget();

        public void Exit()
        {
            _shipSetupMenu.OnSetupComplete -= SwitchState;
            _shipSetupMenu.SceneCleanUp();
        }

        public void Init(IGameStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        private async UniTaskVoid StartSetupAsync()
        {
            using var cts = _tokenProvider.CreateLocalCts();

            await _assetsProvider.WarmUpCurrentSceneAsync();
            await InitSceneAsync();
            _soundService.PlayMusic();
            await _curtain.SetCurtainVisibleAsync(false, cts.Token);
        }

        private async UniTask InitSceneAsync()
        {
            await _shipsInitializer.CreateShipsAsync();
            await SetupUiAsync();
        }

        private async UniTask SetupUiAsync()
        {
            await _uiFactory.CreateRootAsync();
            _shipSetupMenu = await _uiFactory.CreateShipSetupMenuAsync();
            _shipSetupMenu.Init(_staticDataService, _uiFactory, _uiConfig);
            await _shipSetupMenu.SetupUiAsync(_shipsInitializer.Ships.Keys);
            _shipSetupMenu.OnSetupComplete += SwitchState;
            _cleaner.AddCleanable(_shipSetupMenu);
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