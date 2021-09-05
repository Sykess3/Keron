using CodeBase.Data;

namespace CodeBase.Infrastructure
{
    public interface ISavedProgressCleanable : ISavedProgressReader
    {
        void UpdateProgress(ref PlayerProgress to);
    }
}