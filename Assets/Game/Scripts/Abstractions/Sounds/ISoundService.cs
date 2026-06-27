using Equipment;

namespace Sounds
{
    public interface ISoundService
    {
        void Init();
        void PlayMusic();
        void PlayShoot(WeaponType weaponType);
        void StopAll();
    }
}
