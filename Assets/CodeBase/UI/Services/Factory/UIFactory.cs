using System.Threading.Tasks;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services.Ads;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.StaticData;
using CodeBase.StaticData;
using CodeBase.UI.Services.Windows;
using CodeBase.UI.Windows;
using UnityEngine;

namespace CodeBase.UI.Services.Factory
{
    public class UIFactory : IUIFactory
    {
        private readonly IAssets _assets;
        private readonly IStaticDataService _staticData;
        private readonly IPersistentProgressService _progressService;
        private readonly IAdsService _adService;
        private Transform _uiRoot;

        public UIFactory(IAssets assets,
            IStaticDataService staticData,
            IPersistentProgressService progressService,
            IAdsService adService)
        {
            _assets = assets;
            _staticData = staticData;
            _progressService = progressService;
            _adService = adService;
        }

        public async Task CreateUIRoot()
        {
            GameObject uiRoot = await _assets.Instantiate(AssetAddress.UIRoot);
            _uiRoot = uiRoot.transform;
        }

        public void CreateShop()
        {
            WindowConfig windowConfig = _staticData.ForWindow(WindowId.Shop);

            ShopWindow window = Object.Instantiate(windowConfig.WindowPrefab, _uiRoot) as ShopWindow;
            window.Construct(_progressService, _adService);
        }
    }
}