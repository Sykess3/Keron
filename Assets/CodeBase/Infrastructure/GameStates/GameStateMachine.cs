using System;
using System.Collections.Generic;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services.IAP;
using CodeBase.Infrastructure.Services.SaveLoad;
using CodeBase.Logic;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.StaticData;
using CodeBase.UI.Services.Factory;
using Zenject;

namespace CodeBase.Infrastructure.GameStates
{
    public class GameStateMachine : IGameStateMachine
    {
        private readonly Dictionary<Type, IExitableGameState> _states = new Dictionary<Type, IExitableGameState>();
        private IExitableGameState _currentState;
        
        public GameStateMachine(IInstantiator diContainer)
        {
            _states[typeof(InitGameWorldState)] = diContainer.Instantiate<InitGameWorldState>();
            _states[typeof(GameLoopState)] = diContainer.Instantiate<GameLoopState>();
            _states[typeof(LoadLevelState)] = diContainer.Instantiate<LoadLevelState>();
            _states[typeof(LoadProgressState)] = diContainer.Instantiate<LoadProgressState>();
            _states[typeof(StaticDataLoadGameState)] = diContainer.Instantiate<StaticDataLoadGameState>();
        }

        public void Enter<TState>() where TState : class, IGameState
        {
            IGameState state = ChangeState<TState>();
            state.Enter();
        }

        public void Enter<TState, TPayload>(TPayload payload) where TState :
            class, IPayloadedGameState<TPayload>
        {
            IPayloadedGameState<TPayload> state = ChangeState<TState>();
            state.Enter(payload);
        }

        public TState ChangeState<TState>() where TState : class, IExitableGameState
        {
            _currentState?.Exit();
            TState state = GetState<TState>();
            _currentState = state;
            return state;
        }

        public TState GetState<TState>() where TState : class, IExitableGameState
        {
            return _states[typeof(TState)] as TState;
        }
    }
}