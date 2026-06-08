using System.Collections.Generic;
using Equipment.Data;
using Infrastructure;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;


namespace Sounds
{
    internal sealed class SoundService : ISoundService
    {
        private readonly SoundConfig _soundConfig;
        private readonly ISoundFactory _soundFactory;

        private SoundPlayerView _soundPlayer;
        private Dictionary<WeaponType, AudioClip> _weaponShootClips;
        private AudioClip _musicClip;
        private bool _musicIsPlaying;

        [Inject]
        public SoundService(SoundConfig soundConfig, ISoundFactory soundFactory)
        {
            _soundConfig = soundConfig;
            _soundFactory = soundFactory;
        }

        public void Init()
        {
            InitPlayer();
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

        private void InitPlayer()
        {
            if (_soundPlayer == null)
                _soundPlayer = _soundFactory.CreateSoundPlayer();
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
