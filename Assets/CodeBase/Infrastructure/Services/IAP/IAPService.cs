using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeBase.Data;
using CodeBase.Services.PersistentProgress;
using UnityEngine.Purchasing;

namespace CodeBase.Infrastructure.Services.IAP
{
    public class IAPService : IIAPService
    {
        private readonly IPersistentProgressService _persistentProgressService;
        private readonly IAPProvider _iapProvider;

        public bool IsInitialized => _iapProvider.IsInitialized;
        public event Action Initialized;

        public IAPService(IPersistentProgressService persistentProgressService, IAPProvider iapProvider)
        {
            _persistentProgressService = persistentProgressService;
            _iapProvider = iapProvider;
        }

        public async Task Initialize()
        {
            await _iapProvider.Initialize(ProcessPurchase);
            
            _iapProvider.Initialized += () => Initialized?.Invoke();
        }

        public void StartPurchase(string productId) => 
            _iapProvider.StartPurchase(productId);

        public List<ProductDescription> Products() => 
            ProductsDescriptions().ToList();

        private PurchaseProcessingResult ProcessPurchase(Product product)
        {
            ProductConfig config = _iapProvider.Configs[product.definition.id];

            switch (config.ItemType)
            {
                case ItemType.Skull:
                    _persistentProgressService.Progress.Money.Amount += config.Quantity;
                    _persistentProgressService.Progress.PurchaseData.AddIAP(product.definition.id);
                    break;
            }

            return PurchaseProcessingResult.Complete;
        }

        private IEnumerable<ProductDescription> ProductsDescriptions()
        {
            PurchaseData purchaseData = _persistentProgressService.Progress.PurchaseData;
            foreach (string productId in _iapProvider.Products.Keys)
            {
                ProductConfig config = _iapProvider.Configs[productId];
                Product product = _iapProvider.Products[productId];
                
                if (ProductBoughtOut())
                    continue;
                

                yield return new ProductDescription()
                {
                    Id = productId,
                    Config = config,
                    Product = product,
                    AvailablePurchasesLeft = (!purchaseData.BoughtIAPs.Contains(productId))
                        ? config.MaxPurchaseCount
                        : config.MaxPurchaseCount - purchaseData.BoughtIAPs[productId].Count
                };
                
                bool ProductBoughtOut()
                {
                    return purchaseData.BoughtIAPs.Contains(productId) && purchaseData.BoughtIAPs[productId].Count >= config.MaxPurchaseCount;
                }
            }
        }
    }
}