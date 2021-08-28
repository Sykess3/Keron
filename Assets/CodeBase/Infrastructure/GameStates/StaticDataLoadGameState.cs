using System.Threading.Tasks;
using CodeBase.Infrastructure.Services.IAP;
using CodeBase.Services.StaticData;

namespace CodeBase.Infrastructure.GameStates
{
    public class StaticDataLoadGameState : IGameState
    {
        private readonly IGameStateMachine _stateMachine;
        private readonly IStaticDataService _staticDataService;
        private readonly IIAPService _iapService;

        public StaticDataLoadGameState(IGameStateMachine stateMachine, IStaticDataService staticDataService, IIAPService iapService)
        {
            _iapService = iapService;
            _stateMachine = stateMachine;
            _staticDataService = staticDataService;
        }

        public async void Enter()
        {
            await LoadResources();
            _stateMachine.Enter<LoadProgressState>();
        }

        private async Task LoadResources()
        {
            await _staticDataService.Load();
            await _iapService.Initialize();
        }

        public void Exit()
        {
            
        }
    }
}