using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.Services;

namespace CodeBase.Infrastructure.Services.IAP
{
    public interface IIAPService 
    {
        bool IsInitialized { get; }
        event Action Initialized;
        Task Initialize();
        void StartPurchase(string productId);
        List<ProductDescription> Products();
    }
}