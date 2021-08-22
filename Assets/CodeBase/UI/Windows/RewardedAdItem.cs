using CodeBase.Data;
using CodeBase.Infrastructure.Services.Ads;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Windows
{
    public class RewardedAdItem : MonoBehaviour
    {
        [SerializeField] private Button _showAd;
        [SerializeField] private GameObject[] _adActiveObject;
        [SerializeField] private GameObject[] _adInactiveObjects;
        private IAdsService _adService;
        
        private PlayerProgress _progress;
        
        public void Construct(IAdsService adService, PlayerProgress playerProgress)
        {
            _adService = adService;
            _progress = playerProgress;
        }
        
        public void Initialize() => 
            _showAd.onClick.AddListener(OnShowAdButtonClick);

        public void Subscribe() => 
            _adService.RewardedVideoReady += RefreshAdsAvailability;

        public void Cleanup() => 
            _adService.RewardedVideoReady -= RefreshAdsAvailability;

        private void RefreshAdsAvailability()
        {
            bool isVideoReady = _adService.IsRewardedVideoReady;

            foreach (GameObject availableAdObject in _adActiveObject) 
                availableAdObject.SetActive(isVideoReady);

            foreach (GameObject unavailableAdObject in _adInactiveObjects) 
                unavailableAdObject.SetActive(!isVideoReady);
        }

        private void OnShowAdButtonClick() => 
            _adService.ShowRewardedVideo(OnVideoFinished);

        private void OnVideoFinished() => 
            _progress.Money.Amount += _adService.RewardedVideo_Reward;
    }
}