using Sounds;
using Zenject;

namespace Infrastructure
{
    internal sealed class SoundFactory : ISoundFactory
    {
        private readonly IAssetsInstantiator _instantiator;
        private readonly SoundConfig _soundConfig;

        [Inject]
        public SoundFactory(IAssetsInstantiator instantiator, SoundConfig soundConfig)
        {
            _instantiator = instantiator;
            _soundConfig = soundConfig;
        }

        public SoundPlayerView CreateSoundPlayer() 
            => _instantiator.Create(_soundConfig.SoundPlayerPrefab);
    }
}