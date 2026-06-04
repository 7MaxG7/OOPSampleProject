using Cysharp.Threading.Tasks;
using Enums;

namespace Sounds
{
    public interface ISoundPlayer
    {
        UniTask InitAsync();
        void PlayMusic();
        void PlayShoot(WeaponType weaponType);
        void StopAll();
    }
}
