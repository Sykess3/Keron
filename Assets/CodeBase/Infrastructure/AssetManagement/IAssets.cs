using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.Services;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CodeBase.Infrastructure.AssetManagement
{
    public interface IAssets : IService
    {
        Task<GameObject> Instantiate(string path, Vector3 at);
        Task<GameObject> Instantiate(string path);
        Task<T> LoadByName<T>(AssetReferenceGameObject assetReference) where T : class;
        void CleanUp();
        Task<T> LoadByName<T>(string assetAddress) where T : class;
        void Initialize();
        Task<IList<T>> LoadByLabel<T>(string assetAddress) where T : class;
    }
}