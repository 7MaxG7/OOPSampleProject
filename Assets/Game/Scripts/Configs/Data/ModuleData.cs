using Enums;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Configs.Data
{
    [CreateAssetMenu(menuName = "Configs/" + nameof(ModuleData), fileName = nameof(ModuleData))]
    public class ModuleData : ScriptableObject
    {
        [SerializeField] private ModuleType _moduleType;
        [SerializeField] private bool _isActive;
        [SerializeField] private EffectType _effectType;
        [SerializeField] private MathType _mathType;
        [SerializeField] private float _value;
        [SerializeField] private AssetReference _prefab;
        [SerializeField] private Sprite _icon;

        public ModuleType ModuleType => _moduleType;
        public EffectType EffectType => _effectType;
        public MathType MathType => _mathType;
        public float Value => _value;
        public Sprite Icon => _icon;
        public AssetReference Prefab => _prefab;
        public bool IsActive => _isActive;
    }
}