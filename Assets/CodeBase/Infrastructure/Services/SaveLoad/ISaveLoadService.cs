using CodeBase.Data;
using CodeBase.Infrastructure.Factory;
using CodeBase.Services;

namespace CodeBase.Infrastructure.Services.SaveLoad
{
    public interface ISaveLoadService 
    {
        void SaveProgress();
        PlayerProgress LoadProgress();
    }
}