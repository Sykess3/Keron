using CodeBase.Data;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services.Ads;
using CodeBase.Infrastructure.Services.IAP;
using CodeBase.Services.PersistentProgress;
using TMPro;
using UnityEngine;

namespace CodeBase.UI.Windows
{
    class ShopWindow : WindowBase
    {
        [SerializeField] private TextMeshProUGUI _skullText;
        [SerializeField] private RewardedAdItem _rewardedAd;
        [SerializeField] private ShopItemsContainer _shopItemsContainer;

        public void Construct(
            IPersistentProgressService playerProgress, 
            IAdsService adsService,
            IPersistentProgressService _progressService, 
            IAssets _assets, 
            IIAPService _iapService)
        {
            base.Construct(playerProgress);
            
            _rewardedAd.Construct(adsService, Progress);
            _shopItemsContainer.Construct(_progressService, _iapService, _assets);
            
        }

        protected override void Initialize()
        {
            _rewardedAd.Initialize();
            _shopItemsContainer.Initialize();
            UpdateSkullText();
        }

        protected override void SubscribeUpdate()
        {
            _rewardedAd.Subscribe();
            _shopItemsContainer.SubscribeUpdate();
            Progress.Money.Changed += UpdateSkullText;
        }

        protected override void Cleanup()
        {
            _rewardedAd.CleanUp();
            _shopItemsContainer.CleanUp();
            Progress.Money.Changed -= UpdateSkullText;
        }

        private void UpdateSkullText()
        {
            _skullText.text = Progress.Money.Amount.ToString();
        }
    }
}