using System;
using UnityEngine;
using UnityEngine.Advertisements;

namespace CodeBase.Infrastructure.Services.Ads
{

    public class AdsService : IUnityAdsListener, IAdsService
    {
        private const string AndroidRewardedVideo = "Rewarded_Android";
        private const string IOSRewardedVideo = "Rewarded_iOS";
        private const string IOSGameId = "4262662";
        private const string AndroidGameId = "4262663";
        
        private string _gameId;
        private string _rewardedVideo;
        private Action _onVideoFinished;

        public event Action RewardedVideoReady;
        
        public bool IsRewardedVideoReady => Advertisement.IsReady(_rewardedVideo);
        public int RewardedVideo_Reward => 5;

        public void Initialize()
        {
            SetupPlatformSettings();
            Advertisement.AddListener(this);
            Advertisement.Initialize(_gameId);
        }

        public void ShowRewardedVideo(Action onVideoFinished)
        {
            Advertisement.Show(_rewardedVideo);

            _onVideoFinished = onVideoFinished;
        }

        public void OnUnityAdsReady(string placementId)
        {
            Debug.Log($"OnUnityAdsReady {placementId}");

            if (placementId == _rewardedVideo) 
                RewardedVideoReady?.Invoke();
        }

        public void OnUnityAdsDidError(string message) => 
            Debug.LogError($"OnUnityAdsDidFinish {message}");

        public void OnUnityAdsDidStart(string placementId) => 
            Debug.Log($"OnUnityAdsDidStart {placementId}");

        public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
        {
            switch (showResult)
            {
                case ShowResult.Failed:
                    Debug.LogError($"OnUnityAdsDidFinish {showResult}");
                    break;
                case ShowResult.Skipped:
                    Debug.LogError($"OnUnityAdsDidFinish {showResult}");
                    break;
                case ShowResult.Finished:
                    _onVideoFinished?.Invoke();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(showResult), showResult, null);
            }
        }

        private void SetupPlatformSettings()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    _gameId = AndroidGameId;
                    _rewardedVideo = AndroidRewardedVideo;
                    break;
                case RuntimePlatform.IPhonePlayer:
                    _gameId = IOSGameId;
                    _rewardedVideo = IOSRewardedVideo;
                    break;

                case RuntimePlatform.WindowsEditor:
                    _gameId = IOSGameId;
                    _rewardedVideo = IOSRewardedVideo;
                    break;
                default:
                    Debug.Log("Unsupported platform for ads");
                    break;
            }
        }
    }
}