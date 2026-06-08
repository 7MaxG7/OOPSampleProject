using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Infrastructure
{
    public interface IAssetsInstantiator
    {
        T Create<T>(T prefab, Transform parent = null) where T : MonoBehaviour;
        T Create<T>(T prefab, Vector3 position, Quaternion rotation, Transform parent = null) where T : MonoBehaviour;
        GameObject Create(GameObject prefab, Transform parent = null);
        UniTask<T> CreateAsync<T>(AssetReference assetReference, Transform parent = null) where T : MonoBehaviour;
        UniTask<T> CreateAsync<T>(AssetReference assetReference, Vector3 position, Quaternion rotation
            , Transform parent = null) where T : MonoBehaviour;
        UniTask<GameObject> CreateAsync(AssetReference assetReference, Transform parent = null);
    }
}