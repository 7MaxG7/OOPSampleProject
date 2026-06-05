using Infrastructure.ControllersHolder;

namespace Infrastructure.GameStates
{
    public interface IGameStateMachine : ICleanable
    {
        void Enter<TState>() where TState : class, IGameState;
        void Init();
    }
}