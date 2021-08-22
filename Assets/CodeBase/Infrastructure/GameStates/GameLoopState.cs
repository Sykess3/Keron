namespace CodeBase.Infrastructure.GameStates
{
    public class GameLoopState : IGameState
    {
        private readonly GameStateMachine _stateMachine;

        public GameLoopState(GameStateMachine gameStateMachine)
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