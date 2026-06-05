using UnityEngine;


namespace Sounds
{
    public sealed class SoundPlayerView : MonoBehaviour
    {
        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private AudioSource _sfxSource;
        
        public bool MusicLoop
        {
            set => _musicSource.loop = value;
        }
        
        
        public void PlaySound(AudioClip audioClip)
            => _sfxSource.PlayOneShot(audioClip);

        public void StopMusic()
            => _musicSource.Stop();

        public void PlayMusic(AudioClip audioClip)
        {
            _musicSource.clip = audioClip;
            _musicSource.Play();
        }

        public void StopSound()
            => _sfxSource.Stop();
    }
}