using Zenject;


namespace Infrastructure
{
    internal sealed class Game
    {
        public IUpdater Updater { get; }
        public ICleaner Cleaner { get; }

        private readonly IGameStateMachine _gameStateMachine;
        
        [Inject]
        public Game(IUpdater updater, ICleaner cleaner, IGameStateMachine gameStateMachine)
        {
            Updater = updater;
            Cleaner = cleaner;
            _gameStateMachine = gameStateMachine;
            Cleaner.AddCleanable(Updater);
        }

        public void Init()
        {
            _gameStateMachine.Init();
            _gameStateMachine.Enter<GameBootstrapState>();
        }
    }
}