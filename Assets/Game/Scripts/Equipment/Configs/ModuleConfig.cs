using Equipment.Data;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Equipment
{
    [CreateAssetMenu(menuName = "Configs/" + nameof(ModuleConfig), fileName = nameof(ModuleConfig))]
    public class ModuleConfig : ScriptableObject
    {
        [SerializeField] private ModuleType _moduleType;
        [SerializeField] private bool _isActive;
        [SerializeField] private BuffParamType _buffParamType;
        [SerializeField] private BuffRelativenessType _buffRelativenessType;
        [SerializeField] private float _value;
        [SerializeField] private AssetReference _prefab;
        [SerializeField] private Sprite _icon;

        public ModuleType ModuleType => _moduleType;
        public BuffParamType BuffParamType => _buffParamType;
        public BuffRelativenessType BuffRelativenessType => _buffRelativenessType;
        public float Value => _value;
        public Sprite Icon => _icon;
        public AssetReference Prefab => _prefab;
        public bool IsActive => _isActive;
    }
}