namespace Abstractions.Infrastructure
{
    public interface IGameStateMachine : ICleanable
    {
        void Enter<TState>() where TState : class, IGameState;
        void Init();
    }
}