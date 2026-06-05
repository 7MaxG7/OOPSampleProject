using Cysharp.Threading.Tasks;
using Sounds;

namespace Infrastructure
{
    public interface IServicesFactory
    {
        UniTask<SoundPlayerView> CreateSoundPlayerAsync();
    }
}
