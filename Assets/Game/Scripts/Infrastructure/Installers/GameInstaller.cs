using Battle;
using Equipment;
using Ships;
using Sounds;
using Zenject;

namespace Infrastructure
{
    public sealed class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IShipsInitializer>().To<ShipsInitializer>().AsSingle();
            Container.Bind<IShipsFactory>().To<ShipsFactory>().AsSingle();
            Container.Bind<IShipViewFactory>().To<ShipViewFactory>().AsSingle();
            Container.Bind<IAmmoFactory>().To<AmmoFactory>().AsSingle();
            Container.Bind<IWeaponFactory>().To<WeaponFactory>().AsSingle();
            Container.Bind<IModuleFactory>().To<ModuleFactory>().AsSingle();
            Container.Bind<IShipUpgrader>().To<ShipUpgrader>().AsSingle();
            Container.Bind<IWinnerDefiner>().To<WinnerDefiner>().AsSingle();
            Container.Bind<IDamageableIdentifier>().To<DamageableIdentifier>().AsSingle();
            Container.Bind<IDamageHandler>().To<DamageHandler>().AsSingle();
            Container.Bind<IShipConfigurator>().To<ShipConfigurator>().AsSingle();
            
            Container.Bind<ISoundService>().To<SoundService>().AsSingle();
        }
    }
}