using Cysharp.Threading.Tasks;
using DG.Tweening;
using Ships;
using Sounds;
using Ui;
using Utils;
using Zenject;

namespace Infrastructure.GameStates
{
    internal sealed class GameBootstrapState : IGameState
    {
        private readonly IStaticDataService _staticDataService;
        private readonly ISceneLoader _sceneLoader;
        private readonly ICurtain _curtain;
        private readonly IAssetsProvider _assetsProvider;
        private readonly ISoundService _soundService;
        private readonly RulesConfig _rulesConfig;
        private readonly ICancellationTokenProvider _tokenProvider;
        private readonly IShipConfigurationsHolder _configurationsHolder;
        private IGameStateMachine _stateMachine;

        [Inject]
        public GameBootstrapState(IStaticDataService staticDataService, ISceneLoader sceneLoader, ICurtain curtain
            , IAssetsProvider assetsProvider, ISoundService soundService, RulesConfig rulesConfig, ICancellationTokenProvider tokenProvider
            , IShipConfigurationsHolder configurationsHolder)
        {
            _staticDataService = staticDataService;
            _sceneLoader = sceneLoader;
            _curtain = curtain;
            _assetsProvider = assetsProvider;
            _soundService = soundService;
            _rulesConfig = rulesConfig;
            _tokenProvider = tokenProvider;
            _configurationsHolder = configurationsHolder;
        }

        public void Enter()
            => InitAndStartAsync().Forget();

        public void Exit()
        {
        }

        public void Init(IGameStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        private async UniTaskVoid InitAndStartAsync()
        {
            _tokenProvider.Init();
            InitServices();

            var cts = _tokenProvider.CreateLocalCts();
            await _sceneLoader.LoadSceneAsync(Constants.SETUP_SCENE_NAME, cts);
            _stateMachine.Enter<ShipSetupState>();
        }

        private void InitServices()
        {
            DOTween.Init();
            _assetsProvider.Init();
            _curtain.Init();
            _curtain.ShowCurtainInstantly();
            _staticDataService.Init();
            _configurationsHolder.Init(_rulesConfig.Opponents);
            _soundService.Init();
        }
    }
}