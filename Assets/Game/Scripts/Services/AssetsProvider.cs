using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Configs;
using Infrastructure;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;
using static UnityEngine.Object;


namespace Services
{
    internal sealed class AssetsProvider : IAssetsProvider
    {
        private readonly ISceneLoader _sceneLoader;
        private readonly AssetsConfig _assetsConfig;

        private readonly Dictionary<string, AsyncOperationHandle> _loadedAssets = new();
        private readonly Dictionary<string, AsyncOperationHandle> _loadedDontDestroyAssets = new();
        private readonly List<AsyncOperationHandle> _handles = new();
        private readonly List<AsyncOperationHandle> _dontDestroyHandles = new();
        private bool _isCleaned;


        [Inject]
        public AssetsProvider(ISceneLoader sceneLoader, ICleaner cleaner, AssetsConfig assetsConfig)
        {
            _sceneLoader = sceneLoader;
            _assetsConfig = assetsConfig;
            cleaner.AddCleanable(this);
        }
                
        public void CleanUp()
        {
            SceneCleanUp();
            
            foreach (var handle in _dontDestroyHandles) 
                Addressables.Release(handle);
            _dontDestroyHandles.Clear();
            _loadedDontDestroyAssets.Clear();
        }

        public void SceneCleanUp()
        {
            if (_isCleaned)
                return;

            _isCleaned = true;
            foreach (var handle in _handles) 
                Addressables.Release(handle);
            _handles.Clear();
            _loadedAssets.Clear();
        }

        public void Init()
        {
            Addressables.InitializeAsync();
        }

        public async UniTask WarmUpCurrentSceneAsync()
        {
            var sceneName = _sceneLoader.GetCurrentSceneName();
            var assetReferences = _assetsConfig.GetAssetReferencesForState(sceneName);
            foreach (var reference in assetReferences) 
                await LoadAsync(reference);
        }

        public async UniTask<T> CreateInstanceAsync<T>(AssetReference assetReference,
            Transform parent = null, bool isDontDestroyAsset = false) where T : MonoBehaviour
            => await CreateInstanceAsync<T>(assetReference, Vector3.zero, Quaternion.identity, parent, false, isDontDestroyAsset);

        public async UniTask<T> CreateInstanceAsync<T>(AssetReference assetReference, Vector3 position, Quaternion rotation
            , Transform parent = null, bool isPositioned = true, bool isDontDestroyAsset = false) where T : MonoBehaviour
        {
            var prefab = await LoadAsync(assetReference, isDontDestroyAsset);
            return isPositioned
                ? Instantiate(prefab, position, rotation, parent).GetComponent<T>()
                : Instantiate(prefab, parent).GetComponent<T>();
        }

        public async UniTask<GameObject> CreateInstanceAsync(AssetReference assetReference, Transform parent = null)
        {
            var prefab = await LoadAsync(assetReference);
            return Instantiate(prefab, parent);
        }

        private async UniTask<GameObject> LoadAsync(AssetReference assetReference, bool isDontDestroyAsset = false)
        {
            _isCleaned = false;
            var loadedAssets = isDontDestroyAsset ? _loadedDontDestroyAssets : _loadedAssets;
            var handles = isDontDestroyAsset ? _dontDestroyHandles : _handles;
            
            if (loadedAssets.TryGetValue(assetReference.AssetGUID, out var loadedHandle))
                return loadedHandle.Result as GameObject;
            
            var handle = Addressables.LoadAssetAsync<GameObject>(assetReference);
            handle.Completed += resultHandle =>
            {
                loadedAssets[assetReference.AssetGUID] = resultHandle;
                handles.Add(handle);
            };
            return await handle.Task;
        }
    }
}