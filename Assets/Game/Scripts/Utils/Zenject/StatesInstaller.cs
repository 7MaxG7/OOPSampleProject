using Infrastructure;
using Zenject;

namespace Utils.Zenject
{
    internal sealed class StatesInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<GameBootstrapState>().AsSingle();
            Container.Bind<ShipSetupState>().AsSingle();
            Container.Bind<LoadBattleState>().AsSingle();
            Container.Bind<RunBattleState>().AsSingle();
            Container.Bind<LeaveBattleState>().AsSingle();

            Container.Bind<IGameStateMachine>().To<GameStateMachine>().AsSingle();
        }
    }
}