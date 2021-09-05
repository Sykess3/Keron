using CodeBase.Data;
using CodeBase.Infrastructure.Services.Ads;
using CodeBase.Services.PersistentProgress;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.UI.Windows
{
    public class RewardedAdItem : MonoBehaviour
    {
        [SerializeField] private Button _showAd;
        [SerializeField] private GameObject[] _adActiveObject;
        [SerializeField] private GameObject[] _adInactiveObjects;
        private IAdsService _adService;
        
        private IPersistentProgressService _persistent;
        
        [Inject]
        private void Construct(IAdsService adService, IPersistentProgressService playerProgress)
        {
            _adService = adService;
            _persistent = playerProgress;
        }
        
        public void Initialize() => 
            _showAd.onClick.AddListener(OnShowAdButtonClick);

        public void Subscribe() => 
            _adService.RewardedVideoReady += RefreshAdsAvailability;

        public void CleanUp() => 
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
            _persistent.Progress.Money.Amount += _adService.RewardedVideo_Reward;
    }
}