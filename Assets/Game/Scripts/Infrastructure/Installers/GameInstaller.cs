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
            Container.Bind<IShipsViewInitializer>().To<ShipsViewInitializer>().AsSingle();
            Container.Bind<IShipsFactory>().To<ShipsFactory>().AsSingle();
            Container.Bind<IShipViewFactory>().To<ShipViewFactory>().AsSingle();
            Container.Bind<IAmmoViewFactory>().To<AmmoViewFactory>().AsSingle();
            Container.Bind<IEquipmentViewFactory>().To<EquipmentViewFactory>().AsSingle();
            Container.Bind<IWeaponShotService>().To<WeaponShotService>().AsSingle();
            Container.Bind<IShipUpgrader>().To<ShipUpgrader>().AsSingle();
            Container.Bind<IWinnerDefiner>().To<WinnerDefiner>().AsSingle();
            Container.Bind<IDamageableIdentifier>().To<DamageableIdentifier>().AsSingle();
            Container.Bind<IDamageHandler>().To<DamageHandler>().AsSingle();
            Container.Bind<IEquipmentIdentifier>().To<EquipmentIdentifier>().AsSingle();
            Container.Bind<IShipConfigurator>().To<ShipConfigurator>().AsSingle();
            
            Container.Bind<EquipmentFactory>().AsSingle();
            Container.Bind<IWeaponFactory>().To<EquipmentFactory>().FromResolve();
            Container.Bind<IModuleFactory>().To<EquipmentFactory>().FromResolve();
            
            Container.Bind<ISoundService>().To<SoundService>().AsSingle();
        }
    }
}