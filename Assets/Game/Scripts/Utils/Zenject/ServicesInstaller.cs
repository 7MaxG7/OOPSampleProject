using Infrastructure;
using Services;
using Ships;
using Sounds;
using Ui;
using UI;
using Zenject;

namespace Utils.Zenject
{
    public sealed class ServicesInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<Game>().AsSingle();
            Container.Bind<ICancellationTokenProvider>().To<CancellationTokenProvider>().AsSingle();
            Container.Bind<ICurtain>().To<Curtain>().AsSingle();
            Container.Bind<IUpdater>().To<Updater>().AsSingle();
            Container.Bind<ICleaner>().To<Cleaner>().AsSingle();
            Container.Bind<IShipsInitializer>().To<ShipsInitializer>().AsSingle();
            Container.Bind<IStaticDataService>().To<StaticDataService>().AsSingle();
            Container.Bind<IAssetsProvider>().To<AssetsProvider>().AsSingle();
            Container.Bind<ISceneLoader>().To<SceneLoader>().AsSingle();
            Container.Bind<IShipsFactory>().To<ShipsFactory>().AsSingle();
            Container.Bind<IUiFactory>().To<UiFactory>().AsSingle();
            Container.Bind<IAmmoFactory>().To<AmmoFactory>().AsSingle();
            Container.Bind<IWeaponFactory>().To<WeaponFactory>().AsSingle();
            Container.Bind<IModuleFactory>().To<ModuleFactory>().AsSingle();
            Container.Bind<IServicesFactory>().To<ServicesFactory>().AsSingle();
            Container.Bind<IShipUpgrader>().To<ShipUpgrader>().AsSingle();
            Container.Bind<IBattleObserver>().To<BattleObserver>().AsSingle();
            Container.Bind<IDamageHandler>().To<DamageHandler>().AsSingle();
            Container.Bind<ILocationFinder>().To<LocationFinder>().AsSingle();
            Container.Bind<ISoundPlayer>().To<SoundPlayer>().AsSingle();
            Container.Bind<IShipConfigurationsHolder>().To<ShipConfigurationsHolder>().AsSingle();
        }
    }
}