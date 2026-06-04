using Cysharp.Threading.Tasks;
using Abstractions.Infrastructure;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Abstractions.Services
{
    public interface IAssetsProvider : ISceneCleanable
    {
        void Init();
        UniTask WarmUpCurrentSceneAsync();
        UniTask<T> CreateInstanceAsync<T>(AssetReference assetReference, Transform parent = null, bool isDontDestroyAsset = false) where T : MonoBehaviour;
        UniTask<T> CreateInstanceAsync<T>(AssetReference assetReference, Vector3 position, Quaternion rotation
            , Transform parent = null, bool isPositioned = true, bool isDontDestroyAsset = false) where T : MonoBehaviour;
        UniTask<GameObject> CreateInstanceAsync(AssetReference assetReference, Transform parent = null);
    }
}
