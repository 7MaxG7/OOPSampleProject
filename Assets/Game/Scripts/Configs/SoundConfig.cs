using System.Collections.Generic;
using System.Linq;
using Configs.Data;
using Enums;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Configs
{
    [CreateAssetMenu(menuName = "Configs/" + nameof(SoundConfig), fileName = nameof(SoundConfig))]
    public class SoundConfig : ScriptableObject
    {
        [SerializeField] private AssetReference _soundPlayerPrefab;
        [SerializeField] private WeaponSound[] _weaponSounds;
        [SerializeField] private AudioClip _musicClip;

        public AssetReference SoundPlayerPrefab => _soundPlayerPrefab;
        public AudioClip MusicClip => _musicClip;


        public Dictionary<WeaponType, AudioClip> GetWeaponShootingClips()
            => _weaponSounds.ToDictionary(clip => clip.WeaponType, clip => clip.AudioClip);
    }
}