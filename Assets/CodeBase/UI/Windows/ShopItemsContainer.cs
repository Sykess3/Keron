using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services.IAP;
using CodeBase.Services.PersistentProgress;
using UnityEngine;

namespace CodeBase.UI.Windows
{
    public class ShopItemsContainer : MonoBehaviour
    {
        private const string ShopItemPath = "ShopItem";
        [SerializeField] private GameObject[] _unavailableShopObjects;
        [SerializeField] private Transform _parent;

        private IPersistentProgressService _progressService;
        private IIAPService _iapService;
        private IAssets _assets;
        private readonly List<ShopItem> _shopItems = new List<ShopItem>();

        public void Construct(IPersistentProgressService progress, IIAPService iapService, IAssets assets)
        {
            _progressService = progress;
            _iapService = iapService;
            _assets = assets;
        }

        public void Initialize() => 
            RefreshAvailableItems();

        public void SubscribeUpdate()
        {
            _iapService.Initialized += RefreshAvailableItems;
            _progressService.Progress.PurchaseData.Changed += RefreshAvailableItems;
        }

        public void CleanUp()
        {
            _iapService.Initialized -= RefreshAvailableItems;
            _progressService.Progress.PurchaseData.Changed -= RefreshAvailableItems;
        }

        private async void RefreshAvailableItems()
        {
            UpdateUnavailableShopObjects();

            if (!_iapService.IsInitialized)
                return;

            ClearShopItemsInCache();

            await FillShopItemsWithCaching();
        }

        private async Task FillShopItemsWithCaching()
        {
            foreach (ProductDescription product in _iapService.Products())
            {
                GameObject shopItemObject = await _assets.Instantiate(ShopItemPath, _parent);
                ShopItem shopItem = shopItemObject.GetComponent<ShopItem>();
                shopItem.Construct(_assets, _iapService, product);
                shopItem.Initialize();
                
                _shopItems.Add(shopItem);
            }
        }

        private void ClearShopItemsInCache()
        {
            foreach (ShopItem shopItem in _shopItems)
                Destroy(shopItem.gameObject);
            
            _shopItems.Clear();
        }

        private void UpdateUnavailableShopObjects()
        {
            foreach (GameObject shopObject in _unavailableShopObjects)
                shopObject.SetActive(!_iapService.IsInitialized);
        }
    }
}