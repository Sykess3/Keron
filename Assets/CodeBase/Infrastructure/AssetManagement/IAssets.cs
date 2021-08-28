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
        Task<GameObject> Instantiate(string shopItemPath, Transform parent);

        Task<T> LoadSingle<T>(AssetReferenceGameObject assetReference) where T : class;
        void CleanUp();
        Task<T> LoadSingle<T>(string assetAddress) where T : class;
        void Initialize();
        Task<IList<T>> LoadCollection<T>(string assetAddress) where T : class;
        Task<T> LoadSingleForEntireLiceCycle<T>(string assetAddress) where T : class;
        Task<IList<T>> LoadCollectionForEntireLiceCycle<T>(string assetAddress) where T : class;
        
    }
}