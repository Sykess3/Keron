using CodeBase.Data;
using CodeBase.Infrastructure.Factory;
using CodeBase.Services.PersistentProgress;
using UnityEngine;

namespace CodeBase.Infrastructure.Services.SaveLoad
{
    class SaveLoadService : ISaveLoadService
    {
        private const string ProgressKey = "Progress";
        private readonly IPersistentProgressService _progress;
        public IGameFactory GameFactory { get; set; }

        public SaveLoadService(IPersistentProgressService progress)
        {
            _progress = progress;
        }
        
        public void SaveProgress()
        {
            PlayerProgress newProgress = _progress.Progress;
            foreach (var progressWriters in GameFactory.ProgressWritersAndReaders)
                progressWriters.UpdateProgress(to: ref newProgress);
            
            PlayerPrefs.SetString(ProgressKey, newProgress.ToJson());
        }

        public PlayerProgress LoadProgress() =>
            PlayerPrefs.GetString(ProgressKey)?
                .ToDeserialize<PlayerProgress>();
    }
}