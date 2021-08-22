using System.Threading.Tasks;
using CodeBase.Services.StaticData;

namespace CodeBase.Infrastructure.GameStates
{
    public class StaticDataLoadGameState : IGameState
    {
        private readonly IGameStateMachine _stateMachine;
        private readonly IStaticDataService _staticDataService;

        public StaticDataLoadGameState(IGameStateMachine stateMachine, IStaticDataService staticDataService)
        {
            _stateMachine = stateMachine;
            _staticDataService = staticDataService;
        }

        public async void Enter()
        {
            await LoadResources();
            _stateMachine.Enter<LoadProgressState>();
        }

        private async Task LoadResources() => 
            await _staticDataService.Load();

        public void Exit()
        {
            
        }
    }
}