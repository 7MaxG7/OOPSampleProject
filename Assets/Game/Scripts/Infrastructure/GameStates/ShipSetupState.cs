using Configs;
using Cysharp.Threading.Tasks;
using Services;
using Ships;
using Sounds;
using Ui;
using UI.ShipSetup;
using Zenject;


namespace Infrastructure
{
    internal sealed class ShipSetupState : IGameState
    {
        private readonly ICurtain _curtain;
        private readonly IShipsInitializer _shipsInitializer;
        private readonly ISoundPlayer _soundPlayer;
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
            , ISoundPlayer soundPlayer, IAssetsProvider assetsProvider, IUiFactory uiFactory, ICleaner cleaner, UiConfig uiConfig,
            ICancellationTokenProvider tokenProvider)
        {
            _curtain = curtain;
            _shipsInitializer = shipsInitializer;
            _soundPlayer = soundPlayer;
            _staticDataService = staticDataService;
            _assetsProvider = assetsProvider;
            _uiFactory = uiFactory;
            _cleaner = cleaner;
            _uiConfig = uiConfig;
            _tokenProvider = tokenProvider;
        }

        public void Enter()
            => StartSetupAsync().Forget();

        private async UniTaskVoid StartSetupAsync()
        {
            using var cts = _tokenProvider.CreateLocalCts();

            await _assetsProvider.WarmUpCurrentSceneAsync();
            await InitSceneAsync();
            _soundPlayer.PlayMusic();
            await _curtain.SetCurtainVisibleAsync(false, cts.Token);
        }

        public void Exit()
        {
            _shipSetupMenu.OnSetupComplete -= SwitchState;
            _shipSetupMenu.SceneCleanUp();
        }

        public void Init(IGameStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        private async UniTask InitSceneAsync()
        {
            await _uiFactory.PrepareCanvasAsync();
            await _shipsInitializer.PrepareShipsAsync();
            await SetupUiAsync();
        }

        private async UniTask SetupUiAsync()
        {
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