using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.GameStates;
using Zenject;

namespace CodeBase.Infrastructure
{
    public class LocationInstaller : MonoInstaller, IInitializable
    {
        public override void InstallBindings()
        {
            Bind_InstallerInterfaces();
            SetCurrentSceneContainerForGameFactory();
        }

        private void Bind_InstallerInterfaces()
        {
            Container
                .BindInterfacesTo<LocationInstaller>()
                .FromInstance(this)
                .AsSingle();
        }

        public void Initialize() => 
            EnterInitGameWorldState();

        private void SetCurrentSceneContainerForGameFactory() => 
            Container.Resolve<IGameFactory>().SceneContextContainer = Container;

        private void EnterInitGameWorldState() => 
            Container.Resolve<IGameStateMachine>().Enter<InitGameWorldState>();
    }
}
