using CodeBase.Data;
using CodeBase.Infrastructure.Factory;
using CodeBase.Services.PersistentProgress;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure.Services.SaveLoad
{
    class SaveLoadService : ISaveLoadService
    {
        private const string ProgressKey = "Progress";
        private readonly IPersistentProgressService _progress;
        private readonly IProgressWatchersContainer _progressWatchersContainer;

        public SaveLoadService(DiContainer diContainer, IPersistentProgressService progress, IProgressWatchersContainer progressWatchersContainer)
        {
            _progress = progress;
            _progressWatchersContainer = progressWatchersContainer;
        }
        
        public void SaveProgress()
        {
            PlayerProgress newProgress = _progress.Progress;
            
            UpdateProgressForCleanable(ref newProgress);
            UpdateProgressForUnCleanable(ref newProgress);

            PlayerPrefs.SetString(ProgressKey, newProgress.ToJson());
        }

        private void UpdateProgressForCleanable(ref PlayerProgress newProgress)
        {
            foreach (var progressWriters in _progressWatchersContainer.CleanableProgressWriters)
                progressWriters.UpdateProgress(to: ref newProgress);
        }

        private void UpdateProgressForUnCleanable(ref PlayerProgress newProgress)
        {
            foreach (var progressWriters in _progressWatchersContainer.UnCleanableProgressWriters)
                progressWriters.UpdateProgress(to: ref newProgress);
        }

        public PlayerProgress LoadProgress() =>
            PlayerPrefs.GetString(ProgressKey)?
                .ToDeserialize<PlayerProgress>();
    }
}