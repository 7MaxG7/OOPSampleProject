using Equipment;
using Equipment.Data;
using Ships;
using Ships.Data;

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