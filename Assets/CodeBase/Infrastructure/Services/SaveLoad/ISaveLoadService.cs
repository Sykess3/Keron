using CodeBase.Data;
using CodeBase.Infrastructure.Factory;
using CodeBase.Services;

namespace CodeBase.Infrastructure.Services.SaveLoad
{
    public interface ISaveLoadService : IService
    {
        void SaveProgress();
        PlayerProgress LoadProgress();
        IGameFactory GameFactory { get; set; }
    }
}