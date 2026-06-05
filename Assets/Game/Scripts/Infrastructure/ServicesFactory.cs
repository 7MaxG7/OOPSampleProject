using Cysharp.Threading.Tasks;
using Sounds;
using Zenject;

namespace Infrastructure
{
    public sealed class ServicesFactory : IServicesFactory
    {
        private readonly IAssetsProvider _assetsProvider;
        private readonly SoundConfig _soundConfig;


        [Inject]
        public ServicesFactory(IAssetsProvider assetsProvider, SoundConfig soundConfig)
        {
            _assetsProvider = assetsProvider;
            _soundConfig = soundConfig;
        }

        public async UniTask<SoundPlayerView> CreateSoundPlayerAsync() 
            => await _assetsProvider.CreateInstanceAsync<SoundPlayerView>(_soundConfig.SoundPlayerPrefab, isDontDestroyAsset: true);
    }
}