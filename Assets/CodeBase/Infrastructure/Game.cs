using CodeBase.Infrastructure.GameStates;
using CodeBase.Logic;
using CodeBase.Services;

namespace CodeBase.Infrastructure
{
    public class Game
    {
        private readonly IGameStateMachine _stateMachine;

        public Game(ICoroutineRunner coroutineRunner, LoadingCurtain loadingCurtain)
        {
            _stateMachine = new GameStateMachine(
                new SceneLoader(coroutineRunner),
                new InitialSceneLoader(coroutineRunner),
                loadingCurtain,
                AllServices.Container);
        }

        public void EnterBootstrap() => 
            _stateMachine.Enter<BootstrapState>();
    }
}