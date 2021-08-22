using CodeBase.Infrastructure.GameStates;
using CodeBase.Logic;
using CodeBase.Services;
using UnityEngine;

namespace CodeBase.Infrastructure
{
    public class Game
    {
        private readonly GameStateMachine _stateMachine;

        public Game(ICoroutineRunner coroutineRunner, LoadingCurtain loadingCurtain)
        {
            _stateMachine = new GameStateMachine(new SceneLoader(coroutineRunner), loadingCurtain, AllServices.Container);
        }

        public void EnterBootstrap()
        {
            _stateMachine.Enter<BootstrapState>();
        }
    }
}