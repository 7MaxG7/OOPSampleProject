using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Zenject;


namespace Infrastructure
{
    internal sealed class GameStateMachine : IGameStateMachine
    {
        private readonly Dictionary<Type, IGameState> _states;
        private IGameState _currentState;


        [Inject]
        [SuppressMessage("ReSharper", "SuggestBaseTypeForParameterInConstructor")]
        public GameStateMachine(GameBootstrapState gameBootstrapState, ShipSetupState shipSetupState
            , LoadBattleState loadBattleState, RunBattleState runBattleState, LeaveBattleState leaveBattleState
            , ICleaner cleaner)
        {
            _states = new Dictionary<Type, IGameState>
            {
                [typeof(GameBootstrapState)] = gameBootstrapState,
                [typeof(ShipSetupState)] = shipSetupState,
                [typeof(LoadBattleState)] = loadBattleState,
                [typeof(RunBattleState)] = runBattleState,
                [typeof(LeaveBattleState)] = leaveBattleState
            };
            cleaner.AddCleanable(this);
        }

        public void Enter<TState>() where TState : class, IGameState
        {
            var newState = SwitchCurrentState<TState>();
            newState.Enter();
        }

        public void CleanUp()
        {
            _currentState = null;
            _states.Clear();
        }

        public void Init()
        {
            _states[typeof(GameBootstrapState)].Init(this);
            _states[typeof(ShipSetupState)].Init(this);
            _states[typeof(LoadBattleState)].Init(this);
            _states[typeof(RunBattleState)].Init(this);
            _states[typeof(LeaveBattleState)].Init(this);
        }

        private TState SwitchCurrentState<TState>() where TState : class, IGameState
        {
            _currentState?.Exit();

            var newState = _states[typeof(TState)] as TState;
            _currentState = newState;

            return newState;
        }
    }
}