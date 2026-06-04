using System.Collections.Generic;
using Abstractions.Services;
using Configs;
using Cysharp.Threading.Tasks;
using Enums;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;


namespace Sounds
{
    internal sealed class SoundPlayer : ISoundPlayer
    {
        private readonly IServicesFactory _servicesFactory;
        private readonly SoundConfig _soundConfig;
        
        private SoundPlayerView _soundPlayer;
        private Dictionary<WeaponType, AudioClip> _weaponShootClips;
        private AudioClip _musicClip;
        private bool _musicIsPlaying;


        [Inject]
        public SoundPlayer(SoundConfig soundConfig, IServicesFactory servicesFactory)
        {
            _soundConfig = soundConfig;
            _servicesFactory = servicesFactory;
        }

        public async UniTask InitAsync()
        {
            await InitPlayerAsync();
            InitClips();
        }

        public void PlayMusic()
        {
            if (_musicIsPlaying || _musicClip == null)
                return;

            _soundPlayer.StopMusic();
            _soundPlayer.PlayMusic(_musicClip);
            _musicIsPlaying = true;
        }


        public void StopAll()
        {
            _soundPlayer.StopMusic();
            _soundPlayer.StopSound();
            _musicIsPlaying = false;
        }

        public void PlayShoot(WeaponType weaponType)
        {
            if (_weaponShootClips.TryGetValue(weaponType, out var clip))
                _soundPlayer.PlaySound(clip);
        }

        private async UniTask InitPlayerAsync()
        {
            if (_soundPlayer == null)
                _soundPlayer = await _servicesFactory.CreateSoundPlayerAsync();
            _soundPlayer.MusicLoop = true;
            Object.DontDestroyOnLoad(_soundPlayer);
        }

        private void InitClips()
        {
            _weaponShootClips = _soundConfig.GetWeaponShootingClips();
            _musicClip = _soundConfig.MusicClip;
        }
    }
}
