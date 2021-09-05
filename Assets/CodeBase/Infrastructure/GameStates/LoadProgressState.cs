using System;
using CodeBase.Data;
using CodeBase.Infrastructure.Services.SaveLoad;
using CodeBase.Services.PersistentProgress;
using Zenject;

namespace CodeBase.Infrastructure.GameStates
{
    public class LoadProgressState : IGameState
    {
        private readonly ISaveLoadService _saveLoadService;
        private readonly IPersistentProgressService _progressService;
        private readonly LazyInject<IGameStateMachine> _stateMachine;
        
        public LoadProgressState(LazyInject<IGameStateMachine> stateMachine,
            IPersistentProgressService progressService, ISaveLoadService saveLoadService)
        {
            _stateMachine = stateMachine;
            _progressService = progressService;
            _saveLoadService = saveLoadService;
        }

        public void Enter()
        {
            LoadProgressOrInitNew();

            _stateMachine.Value.Enter<LoadLevelState, string>(GetSavedLevel());
        }

        public void Exit()
        {
        }

        private void LoadProgressOrInitNew()
        {
            _progressService.Progress =
                _saveLoadService.LoadProgress()
                ?? NewProgress();
        }

        private PlayerProgress NewProgress()
        {
            WorldData worldData = new WorldData(initialLevel: "Graveyard");
            State heroState = new State(maxHP: 100f);
            Stats heroStats = new Stats(1f, 0.5f);
            PlayerProgress playerProgress = new PlayerProgress(worldData, heroState, heroStats);
            return playerProgress;
        }

        private string GetSavedLevel() =>
            _progressService.Progress.WorldData.PositionOnLevel.Level;
    }
}