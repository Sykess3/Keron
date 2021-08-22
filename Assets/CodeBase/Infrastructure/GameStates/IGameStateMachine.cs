using CodeBase.Services;

namespace CodeBase.Infrastructure.GameStates
{
    public interface IGameStateMachine : IService
    {
        void Enter<TState>() where TState : class, IGameState;

        void Enter<TState, TPayload>(TPayload payload) where TState : 
            class, IPayloadedGameState<TPayload>;

        TState ChangeState<TState>() where TState : class, IExitableGameState;
        TState GetState<TState>() where TState : class, IExitableGameState;
    }
}