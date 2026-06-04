using Cysharp.Threading.Tasks;
using Sounds;

namespace Services
{
    public interface IServicesFactory
    {
        UniTask<SoundPlayerView> CreateSoundPlayerAsync();
    }
}
