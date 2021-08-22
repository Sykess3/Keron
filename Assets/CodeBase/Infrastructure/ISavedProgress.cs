using CodeBase.Data;

namespace CodeBase.Infrastructure
{
    public interface ISavedProgress : ISavedProgressReader
    {
        void UpdateProgress(ref PlayerProgress to);
    }
}