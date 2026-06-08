using Infrastructure.ControllersHolder;
using Infrastructure.GameStates;
using Infrastructure.Wrappers;
using Zenject;

namespace Infrastructure
{
    public sealed class GameInfrastructureInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            // States
            Container.Bind<IGameStateMachine>().To<GameStateMachine>().AsSingle();
            Container.Bind<GameBootstrapState>().AsSingle();
            Container.Bind<ShipSetupState>().AsSingle();
            Container.Bind<LoadBattleState>().AsSingle();
            Container.Bind<RunBattleState>().AsSingle();
            Container.Bind<LeaveBattleState>().AsSingle();

            // Lifetime
            Container.Bind<Game>().AsSingle();
            Container.Bind<IUpdater>().To<Updater>().AsSingle();
            Container.Bind<ICleaner>().To<Cleaner>().AsSingle();
   
            // Other
            Container.Bind<ICancellationTokenProvider>().To<CancellationTokenProvider>().AsSingle();
            Container.Bind<IStaticDataService>().To<StaticDataService>().AsSingle();
            Container.Bind<IAssetsProvider>().To<AssetsProvider>().AsSingle();
            Container.Bind<ISceneLoader>().To<SceneLoader>().AsSingle();
            
            Container.Bind<IServicesFactory>().To<ServicesFactory>().AsSingle();
        }
    }
}