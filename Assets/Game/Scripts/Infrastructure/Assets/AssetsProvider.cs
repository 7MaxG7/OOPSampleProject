using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;


namespace Infrastructure
{
    internal sealed class AssetsProvider : IAssetsProvider
    {
        private readonly ISceneLoader _sceneLoader;
        private readonly AssetsConfig _assetsConfig;

        private readonly Dictionary<string, AsyncOperationHandle> _loadedAssets = new();
        private readonly List<AsyncOperationHandle> _handles = new();
        private bool _isCleaned;

        [Inject]
        public AssetsProvider(ISceneLoader sceneLoader, ICleaner cleaner, AssetsConfig assetsConfig)
        {
            _sceneLoader = sceneLoader;
            _assetsConfig = assetsConfig;
            cleaner.AddCleanable(this);
        }

        public void Init()
        {
            Addressables.InitializeAsync();
        }

        public void CleanUp()
        {
            SceneCleanUp();
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

        public async UniTask WarmUpCurrentSceneAsync()
        {
            var sceneName = _sceneLoader.GetCurrentSceneName();
            var assetReferences = _assetsConfig.GetAssetReferencesForState(sceneName);
            foreach (var reference in assetReferences) 
                await LoadAsync(reference);
        }

        public async UniTask<GameObject> LoadAsync(AssetReference assetReference)
        {
            _isCleaned = false;

            if (_loadedAssets.TryGetValue(assetReference.AssetGUID, out var loadedHandle))
                return loadedHandle.Result as GameObject;
            
            var handle = Addressables.LoadAssetAsync<GameObject>(assetReference);
            handle.Completed += resultHandle =>
            {
                _loadedAssets[assetReference.AssetGUID] = resultHandle;
                _handles.Add(handle);
            };
            return await handle.Task;
        }
    }
}