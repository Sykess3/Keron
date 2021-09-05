using System.Collections.Generic;
using CodeBase.Infrastructure;
using UnityEngine;

namespace CodeBase.Services.PersistentProgress
{
    public class ProgressWatchers : IProgressWatchersContainer, IProgressWatchers
    {
        private readonly List<ISavedProgressReader> _progressReaders = new List<ISavedProgressReader>();
        
        private readonly List<ISavedProgressCleanable> _cleanableProgressWriters = new List<ISavedProgressCleanable>();
        private readonly List<ISavedProgressWithoutCleanup> _unCleanableProgressWriters = new List<ISavedProgressWithoutCleanup>();
        private readonly IPersistentProgressService _persistentProgressService;

        public IEnumerable<ISavedProgressReader> ProgressReaders => _progressReaders;

        public IEnumerable<ISavedProgressCleanable> CleanableProgressWriters => _cleanableProgressWriters;
        public IEnumerable<ISavedProgressWithoutCleanup> UnCleanableProgressWriters => _unCleanableProgressWriters;


        public ProgressWatchers(IPersistentProgressService persistentProgressService)
        {
            _persistentProgressService = persistentProgressService;
        }
        

        public void Register(GameObject gameObject)
        {
            foreach (var progress in gameObject.GetComponentsInChildren<ISavedProgressReader>())
                InternalRegister(progress);
        }


        public void InformProgressReaders()
        {
            foreach (var progressReader in _progressReaders)
                progressReader.LoadProgress(@from: _persistentProgressService.Progress);
        }

        public void CleanUp()
        {
            CleanUpProgressReaders();
            _cleanableProgressWriters.Clear();
        }

        private void InternalRegister(ISavedProgressReader progressReader)
        {
            if (progressReader is ISavedProgressWithoutCleanup progressWriterUnCleanable)
                _unCleanableProgressWriters.Add(progressWriterUnCleanable);
            else if(progressReader is ISavedProgressCleanable progressWriterCleanable)
                _cleanableProgressWriters.Add(progressWriterCleanable);
            
            _progressReaders.Add(progressReader);
        }

        private void CleanUpProgressReaders()
        {
            foreach (var progressWriter in _cleanableProgressWriters)
                _progressReaders.Remove(progressWriter);
        }
    }
}