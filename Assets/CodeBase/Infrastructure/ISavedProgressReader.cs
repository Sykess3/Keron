using CodeBase.Data;

namespace CodeBase.Infrastructure
{
    public interface ISavedProgressReader
    {
        void LoadProgress(PlayerProgress @from);
    }
}