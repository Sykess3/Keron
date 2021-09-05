using System.Threading.Tasks;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Services.StaticData;
using CodeBase.StaticData;
using CodeBase.UI.Services.Windows;
using CodeBase.UI.Windows;
using UnityEngine;
using Zenject;

namespace CodeBase.UI.Services.Factory
{
    public class UIFactory : IUIFactory
    {
        private readonly IAssets _assets;
        private readonly IStaticDataService _staticData;
        private Transform _uiRoot;
        private readonly DiContainer _diContainer;

        public UIFactory(
            IAssets assets,
            IStaticDataService staticData,
            DiContainer diContainer)
        {
            _assets = assets;
            _staticData = staticData;
            _diContainer = diContainer;
        }

        public async Task CreateUIRoot()
        {
            GameObject uiRoot = await _assets.LoadSingle<GameObject>(AssetAddress.UIRoot);
            _uiRoot = uiRoot.transform;
        }

        public void CreateShop()
        {
            WindowConfig windowConfig = _staticData.ForWindow(WindowId.Shop);

            InstantiatePrefabForComponent<ShopWindow>(windowConfig.WindowPrefab, _uiRoot);
        }

        private T InstantiatePrefabForComponent<T>(Object prefab, Transform under) => 
            _diContainer.InstantiatePrefabForComponent<T>(prefab, under);
    }
}