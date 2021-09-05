using CodeBase.Data;

namespace CodeBase.Infrastructure
{
    public interface ISavedProgressWithoutCleanup : ISavedProgressReader
    {
        void UpdateProgress(ref PlayerProgress to);
    }
}