using System;
using System.Threading.Tasks;
using CodeBase.Infrastructure.Services.IAP;
using CodeBase.Services.StaticData;
using Zenject;

namespace CodeBase.Infrastructure.GameStates
{
    public class StaticDataLoadGameState : IGameState
    {
        private readonly LazyInject<IGameStateMachine> _stateMachine;
        private readonly IStaticDataService _staticDataService;
        private readonly IIAPService _iapService;
        
        
        public StaticDataLoadGameState(LazyInject<IGameStateMachine> stateMachine,
            IStaticDataService staticDataService, IIAPService iapService)
        {
            _stateMachine = stateMachine;
            _iapService = iapService;
            _staticDataService = staticDataService;
        }

        public async void Enter()
        {
            await LoadResources();
            _stateMachine.Value.Enter<LoadProgressState>();
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