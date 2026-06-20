using Ui;
using UI;
using Zenject;

namespace Infrastructure
{
    public sealed class GameUIInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ICurtain>().To<Curtain>().AsSingle();
            Container.Bind<IUiFactory>().To<UiFactory>().AsSingle();
            Container.Bind<IShipSetupUIService>().To<ShipSetupUIService>().AsSingle();
        }
    }
}