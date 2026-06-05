using System.Threading;
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
        private readonly ISoundPlayer _soundPlayer;
        private readonly RulesConfig _rulesConfig;
        private readonly ICancellationTokenProvider _tokenProvider;
        private readonly IShipConfigurationsHolder _configurationsHolder;
        private IGameStateMachine _stateMachine;


        [Inject]
        public GameBootstrapState(IStaticDataService staticDataService, ISceneLoader sceneLoader, ICurtain curtain
            , IAssetsProvider assetsProvider, ISoundPlayer soundPlayer, RulesConfig rulesConfig, ICancellationTokenProvider tokenProvider
            , IShipConfigurationsHolder configurationsHolder)
        {
            _staticDataService = staticDataService;
            _sceneLoader = sceneLoader;
            _curtain = curtain;
            _assetsProvider = assetsProvider;
            _soundPlayer = soundPlayer;
            _rulesConfig = rulesConfig;
            _tokenProvider = tokenProvider;
            _configurationsHolder = configurationsHolder;
        }

        public void Enter()
            => InitAndStart().Forget();

        public void Exit()
        {
        }

        public void Init(IGameStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        private async UniTaskVoid InitAndStart()
        {
            _tokenProvider.Init();
            using var cts = _tokenProvider.CreateLocalCts();
            await InitServicesAsync(cts);

            await _sceneLoader.LoadSceneAsync(Constants.SETUP_SCENE_NAME, cts);
            _stateMachine.Enter<ShipSetupState>();
        }

        private async UniTask InitServicesAsync(CancellationTokenSource cts)
        {
            DOTween.Init();
            _assetsProvider.Init();
            await _curtain.InitAsync();
            _curtain.ShowCurtainInstantly();
            _staticDataService.Init();
            _configurationsHolder.Init(_rulesConfig.Opponents);
            await _soundPlayer.InitAsync();
        }
    }
}