using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeBase.Infrastructure.Factory;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CodeBase.Infrastructure.AssetManagement
{
    public class AssetsProvider : IAssets
    {
        private readonly Dictionary<string, AsyncOperationHandle> _completedCache = new Dictionary<string, AsyncOperationHandle>();
        private readonly Dictionary<string, List<AsyncOperationHandle>> _handles = new Dictionary<string, List<AsyncOperationHandle>>();

        public void Initialize() => 
            Addressables.InitializeAsync();

        public async Task<T> LoadSingle<T>(AssetReferenceGameObject assetReference) where T : class
        {
            if (_completedCache.TryGetValue(assetReference.AssetGUID, out AsyncOperationHandle completedHandle))
                return completedHandle.Result as T;

            return await RunWitchCacheHandle(
                Addressables.LoadAssetAsync<T>(assetReference),
                assetReference.AssetGUID);
        }

        public async Task<T> LoadSingle<T>(string assetAddress) where T : class
        {
            if (_completedCache.TryGetValue(assetAddress, out AsyncOperationHandle completedHandle))
                return completedHandle.Result as T;

            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(assetAddress);
            return await RunWitchCacheHandle(
                handle,
                assetAddress);
        }
        
        public async Task<T> LoadSingleForEntireLiceCycle<T>(string assetAddress) where T : class
        {
            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(assetAddress);
            return await handle.Task;
        }

        public async Task<IList<T>> LoadCollection<T>(string assetAddress) where T : class
        {
             if (_completedCache.TryGetValue(assetAddress, out AsyncOperationHandle completedHandle))
                 return completedHandle.Result as IList<T>;
             AsyncOperationHandle<IList<T>> handle = Addressables.LoadAssetsAsync<T>(assetAddress, null);
             return await RunWitchCacheHandle(
                 handle,
                 assetAddress);
        }
        public async Task<IList<T>> LoadCollectionForEntireLiceCycle<T>(string assetAddress) where T : class
        {
             AsyncOperationHandle<IList<T>> handle = Addressables.LoadAssetsAsync<T>(assetAddress, null);
             return await handle.Task;
        }

        public async Task<GameObject> Instantiate(string shopItemPath, Transform under) => 
            await Addressables.InstantiateAsync(shopItemPath, under).Task;

        public async Task<GameObject> Instantiate(string address, Vector3 at) => 
            await Addressables.InstantiateAsync(address, at, Quaternion.identity).Task;

        public async Task<GameObject> Instantiate(string address) => 
            await Addressables.InstantiateAsync(address).Task;

        public void CleanUp()
        {
            foreach (var handle in _handles.Values.SelectMany(resourceHandles => resourceHandles))
                Addressables.Release(handle);
            
            _completedCache.Clear();
            _handles.Clear();
        }

        private async Task<T> RunWitchCacheHandle<T>(AsyncOperationHandle<T> handle, string cacheKey) where T : class
        {
            handle.Completed += h =>
            {
                _completedCache[cacheKey] = h;
            };

            AddHandle(cacheKey, handle);

            return await handle
                .Task;
        }
        
        private async Task<IList<T>> RunWitchCacheHandle<T>(AsyncOperationHandle<IList<T>> handle, string cacheKey) where T : class
        {
            handle.Completed += h =>
            {
                _completedCache[cacheKey] = h;
            };
            
            AddHandle(cacheKey, handle);
            
            return await handle
                .Task;
        }

        private void AddHandle<T>(string key, AsyncOperationHandle<T> handle) where T : class
        {
            if (!_handles.TryGetValue(key, out List<AsyncOperationHandle> resourcesHandles))
            {
                resourcesHandles = new List<AsyncOperationHandle>();
                _handles[key] = resourcesHandles;
            }

            resourcesHandles.Add(handle);
        }
    }
}