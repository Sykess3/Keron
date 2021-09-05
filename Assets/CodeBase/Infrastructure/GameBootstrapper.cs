using CodeBase.Infrastructure.GameStates;
using CodeBase.Logic;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure
{
    public class GameBootstrapper : MonoBehaviour
    {
        private Game _game;
        private IGameStateMachine _gameStateMachine;

        [Inject]
        private void Construct(IGameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
            _game = new Game(gameStateMachine);
            _game.Run();

            DontDestroyOnLoad(this);
        }
    }
}