using System.Threading.Tasks;

namespace CodeBase.Infrastructure.GameStates
{
    public interface IGameState : IExitableGameState
    {
        void Enter();
    }
    public interface IPayloadedGameState<TPayLoaded> : IExitableGameState
    {
        void Enter(TPayLoaded payLoaded);
    }
    
    public interface IExitableGameState
    {
        void Exit();
    }
}