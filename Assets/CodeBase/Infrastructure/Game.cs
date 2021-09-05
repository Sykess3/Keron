using CodeBase.Infrastructure.GameStates;

namespace CodeBase.Infrastructure
{
    public class Game
    {
        private readonly IGameStateMachine _stateMachine;

        public Game(IGameStateMachine gameStateMachine) => 
            _stateMachine = gameStateMachine;

        public void Run() =>
            _stateMachine.Enter<StaticDataLoadGameState>();

    }
}