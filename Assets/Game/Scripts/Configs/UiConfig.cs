using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Configs
{
    [CreateAssetMenu(menuName = "Configs/" + nameof(UiConfig), fileName = nameof(UiConfig))]
    public class UiConfig : ScriptableObject
    {
        [SerializeField] private AssetReference _rootCanvas;
        [SerializeField] private float _fadeAnimDuration;
        [Header("Curtain")]
        [SerializeField] private AssetReference _curtainPrefab;
        [SerializeField] private float _curtainAnimDuration;
        [Header("Ship setup scene")]
        [SerializeField] private AssetReference _shipSetupMenu;
        [SerializeField] private AssetReference _slotUiPrefab;
        [SerializeField] private AssetReference _shipSlotUiPrefab;
        [Header("Battle scene")]
        [SerializeField] private AssetReference _battleUiPrefab;

        public AssetReference ShipSetupMenu => _shipSetupMenu;
        public AssetReference ShipSlotUiPrefab => _shipSlotUiPrefab;
        public AssetReference SlotUiPrefab => _slotUiPrefab;
        public AssetReference BattleUiPrefab => _battleUiPrefab;
        public AssetReference RootCanvas => _rootCanvas;
        public AssetReference CurtainPrefab => _curtainPrefab;
        public float CurtainAnimDuration => _curtainAnimDuration;
        public float FadeAnimDuration => _fadeAnimDuration;
    }
}