using Zenject;

namespace CodeBase.Infrastructure.GameStates
{
    public class GameLoopState : IGameState
    {
        private readonly LazyInject<IGameStateMachine> _stateMachine;
        public GameLoopState(LazyInject<IGameStateMachine> stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void Enter()
        {
        }

        public void Exit()
        {
        }
    }
}