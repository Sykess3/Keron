using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services.SaveLoad;
using CodeBase.Logic;
using CodeBase.Services;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.StaticData;
using CodeBase.UI.Services.Factory;

namespace CodeBase.Infrastructure.GameStates
{
    public class GameStateMachine : IGameStateMachine
    {
        private readonly Dictionary<Type, IExitableGameState> _states;
        private IExitableGameState _currentState;
        public GameStateMachine(SceneLoader sceneLoader, LoadingCurtain loadingCurtain, AllServices services)
        {
            _states = new Dictionary<Type, IExitableGameState>()
            {
                [typeof(BootstrapState)] = new BootstrapState(
                    this,
                    sceneLoader,
                    services
                    ),
                [typeof(StaticDataLoadGameState)] = new StaticDataLoadGameState(
                    this, 
                    services.Single<IStaticDataService>()
                    ), 
                [typeof(LoadLevelState)] = new LoadLevelState(
                    this,
                    services.Single<IGameFactory>(),
                    services.Single<IPersistentProgressService>(),
                    services.Single<IStaticDataService>(),
                    loadingCurtain, 
                    sceneLoader,
                    services.Single<IUIFactory>()
                    ),
                [typeof(LoadProgressState)] = new LoadProgressState(
                    this, 
                    services.Single<IPersistentProgressService>(), 
                    services.Single<ISaveLoadService>()
                    ),
                
                [typeof(GameLoopState)] = new GameLoopState(this)
            };
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