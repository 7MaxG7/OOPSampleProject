using Ui;
using UI;
using UI.Battle;
using UI.ShipSetup;
using Zenject;

namespace Infrastructure
{
    public sealed class GameUIInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ICurtain>().To<Curtain>().AsSingle();
            Container.Bind<IUIFactory>().To<UiFactory>().AsSingle();
            Container.Bind<IShipSetupUIBuilder>().To<ShipSetupUIBuilder>().AsSingle();
            Container.Bind<ShipSetupUIModel>().AsSingle();
            Container.Bind<IShipSetupUIService>().To<ShipSetupUIService>().AsSingle();
            Container.Bind<IBattleUIBuilder>().To<BattleUIBuilder>().AsSingle();
            Container.Bind<BattleUIModel>().AsSingle();
        }
    }
}