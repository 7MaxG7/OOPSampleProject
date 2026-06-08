using Cysharp.Threading.Tasks;
using Infrastructure.ControllersHolder;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Infrastructure
{
    public interface IAssetsProvider : ISceneCleanable
    {
        void Init();
        UniTask WarmUpCurrentSceneAsync();
        UniTask<GameObject> LoadAsync(AssetReference assetReference);
    }
}
