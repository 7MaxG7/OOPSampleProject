using Configs.Data;
using Enums;

namespace Services
{
    public interface IStaticDataService
    {
        void Init();
        ShipData GetShipData(ShipType shipType);
        WeaponData GetWeaponData(WeaponType weapon);
        ModuleData GetModuleData(ModuleType module);
        WeaponData[] GetAllEnabledWeaponsData();
        ModuleData[] GetAllEnabledModulesData();
    }
}