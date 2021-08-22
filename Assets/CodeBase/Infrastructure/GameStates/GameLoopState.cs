namespace CodeBase.Infrastructure.GameStates
{
    public class GameLoopState : IGameState
    {
        private readonly IGameStateMachine _stateMachine;

        public GameLoopState(IGameStateMachine gameStateMachine)
        {
            _stateMachine = gameStateMachine;
        }

        public void Enter()
        {
        }

        public void Exit()
        {
        }
    }
}