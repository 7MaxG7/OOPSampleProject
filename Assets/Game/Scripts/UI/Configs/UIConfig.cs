using UnityEngine;
using UnityEngine.AddressableAssets;

namespace UI
{
    [CreateAssetMenu(menuName = "Configs/" + nameof(UIConfig), fileName = nameof(UIConfig))]
    public class UIConfig : ScriptableObject
    {
        [SerializeField] private AssetReference _rootCanvas;
        [SerializeField] private float _fadeAnimDuration;
        [Header("Curtain")]
        [SerializeField] private CurtainUIView _curtainPrefab;
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
        public CurtainUIView CurtainPrefab => _curtainPrefab;
        public float CurtainAnimDuration => _curtainAnimDuration;
        public float FadeAnimDuration => _fadeAnimDuration;
    }
}