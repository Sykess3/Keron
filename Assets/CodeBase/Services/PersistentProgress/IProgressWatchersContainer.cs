using System.Collections.Generic;
using CodeBase.Infrastructure;
using UnityEngine;

namespace CodeBase.Services.PersistentProgress
{
    public interface IProgressWatchersContainer
    {
        void CleanUp();
        void InformProgressReaders();
        IEnumerable<ISavedProgressReader> ProgressReaders { get; }
        IEnumerable<ISavedProgressCleanable> CleanableProgressWriters { get; }
        IEnumerable<ISavedProgressWithoutCleanup> UnCleanableProgressWriters { get; }
    }
}