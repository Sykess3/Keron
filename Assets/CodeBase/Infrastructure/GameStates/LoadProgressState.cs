using CodeBase.Data;
using CodeBase.Infrastructure.Services.SaveLoad;
using CodeBase.Services.PersistentProgress;

namespace CodeBase.Infrastructure.GameStates
{
    public class LoadProgressState : IGameState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly ISaveLoadService _saveLoadService;
        private readonly IPersistentProgressService _progressService;

        public LoadProgressState(GameStateMachine gameStateMachine, 
            IPersistentProgressService progressService, ISaveLoadService saveLoadService)
        {
            _gameStateMachine = gameStateMachine;
            _progressService = progressService;
            _saveLoadService = saveLoadService;
        }

        public void Enter()
        {
            LoadProgressOrInitNew();

            _gameStateMachine.Enter<LoadLevelState, string>(GetSavedLevel());
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
            WorldData worldData = new WorldData(initialLevel: "Main");
            State heroState = new State(maxHP: 100f);
            Stats heroStats = new Stats(1f, 0.5f);
            PlayerProgress playerProgress = new PlayerProgress(worldData, heroState, heroStats);
            return playerProgress;
        }

        private string GetSavedLevel() =>
            _progressService.Progress.WorldData.PositionOnLevel.Level;
    }
}