using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeBase.Data;
using CodeBase.Infrastructure.AssetManagement;
using UnityEngine;
using UnityEngine.Purchasing;

namespace CodeBase.Infrastructure.Services.IAP
{
    public class IAPProvider : IStoreListener
    {
        private readonly IAssets _assets;
        private IStoreController _controller;
        private IExtensionProvider _extensions;
        private Func<Product, PurchaseProcessingResult>  _processPurchasing;

        public bool IsInitialized => _controller != null && _extensions != null;
        public event Action Initialized;
        public Dictionary<string, ProductConfig> Configs { get; private set; }
        public Dictionary<string, Product> Products { get; private set; }

        public IAPProvider(IAssets assets)
        {
            _assets = assets;
        }
        
        public async Task Initialize(Func<Product, PurchaseProcessingResult> processPurchasing)
        {
            _processPurchasing = processPurchasing ?? throw new ArgumentException();
            
            ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

            Configs = await Load() ?? new Dictionary<string, ProductConfig>();
            Products = new Dictionary<string, Product>();

            foreach (var config in Configs.Values)
                builder.AddProduct(config.Id, config.ProductType);

            UnityPurchasing.Initialize(this, builder);
        }

        public void StartPurchase(string productId) =>
            _controller.InitiatePurchase(productId);

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            _extensions = extensions;
            _controller = controller;

            LoadAllProducts();

            Debug.Log("OnInitialized success");

            Initialized?.Invoke();
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            Debug.LogWarning("OnInitializeFailed");
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
        {
            return _processPurchasing(purchaseEvent.purchasedProduct);
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
        }

        private void LoadAllProducts()
        {
            foreach (Product product in _controller.products.all)
                Products.Add(product.definition.id, product);
        }

        private async Task<Dictionary<string, ProductConfig>> Load()
        {
            TextAsset configs = await _assets.LoadSingleForEntireLiceCycle<TextAsset>(AssetAddress.IAPConfigs);
            return configs
                .text
                .ToDeserialize<ProductConfigWrapper>()
                .Configs
                .ToDictionary(x => x.Id, x=> x);
        }
    }
}