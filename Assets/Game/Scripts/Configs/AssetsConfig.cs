using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Utils;

namespace Configs
{
    [CreateAssetMenu(menuName = "Configs/" + nameof(AssetsConfig), fileName = nameof(AssetsConfig))]
    public class AssetsConfig : ScriptableObject
    {
        [SerializeField] private AssetReference[] _setupSceneAssets;
        [SerializeField] private AssetReference[] _battleSceneAssets;

        public IEnumerable<AssetReference> GetAssetReferencesForState(string sceneName)
        {
            return sceneName switch
            {
                Constants.SETUP_SCENE_NAME => _setupSceneAssets,
                Constants.BATTLE_SCENE_NAME => _battleSceneAssets,
                _ => Array.Empty<AssetReference>()
            };
        }
    }
}