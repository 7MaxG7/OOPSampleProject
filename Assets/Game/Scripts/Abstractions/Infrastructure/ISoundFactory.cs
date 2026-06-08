using Sounds;

namespace Infrastructure
{
    public interface ISoundFactory
    {
        SoundPlayerView CreateSoundPlayer();
    }
}