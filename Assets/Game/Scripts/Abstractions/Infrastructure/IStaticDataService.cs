using Equipment;
using Ships;

namespace Infrastructure
{
    public interface IStaticDataService
    {
        void Init();
        ShipConfig GetShip(ShipType shipType);
        WeaponConfig GetWeapon(WeaponType weapon);
        ModuleConfig GetModule(ModuleType module);
        WeaponConfig[] GetAllEnabledWeapons();
        ModuleConfig[] GetAllEnabledModules();
    }
}