namespace Infrastructure
{
    public interface IGameState
    {
        void Init(IGameStateMachine gameStateMachine);
        public void Enter();
        public void Exit();
    }
}