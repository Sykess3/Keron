using CodeBase.Data;
using CodeBase.Infrastructure.Services.Ads;
using CodeBase.Services.PersistentProgress;
using TMPro;
using UnityEngine;

namespace CodeBase.UI.Windows
{
    class ShopWindow : WindowBase
    {
        [SerializeField] private TextMeshProUGUI _skullText;
        [SerializeField] private RewardedAdItem _rewardedAd;

        public void Construct(IPersistentProgressService playerProgress, IAdsService adsService)
        {
            base.Construct(playerProgress);
            
            _rewardedAd.Construct(adsService, Progress);
        }

        protected override void Initialize()
        {
            _rewardedAd.Initialize();
            UpdateSkullText();
        }

        protected override void SubscribeUpdate()
        {
            _rewardedAd.Subscribe();
            Progress.Money.Changed += UpdateSkullText;
        }

        protected override void Cleanup()
        {
            _rewardedAd.Cleanup();
            Progress.Money.Changed -= UpdateSkullText;
        }

        private void UpdateSkullText()
        {
            _skullText.text = Progress.Money.Amount.ToString();
        }
    }
}