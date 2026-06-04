using Cysharp.Threading.Tasks;
using Sounds;

namespace Abstractions.Services
{
    public interface IServicesFactory
    {
        UniTask<SoundPlayerView> CreateSoundPlayerAsync();
    }
}
