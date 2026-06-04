using System.Collections.Generic;
using System.Linq;
using Configs.Data;
using Enums;
using UnityEngine;
using Utils;

namespace Services
{
    public sealed class StaticDataService : IStaticDataService
    {
        private Dictionary<ShipType, ShipData> _shipDatas;
        private Dictionary<WeaponType,WeaponData> _weaponDatas;
        private Dictionary<ModuleType,ModuleData> _moduleDatas;


        public void Init()
        {
            _shipDatas = Resources
                .LoadAll<ShipData>(Constants.SHIP_DATA_PATH)
                .ToDictionary(data => data.ShipType, data => data);
            _weaponDatas = Resources
                .LoadAll<WeaponData>(Constants.WEAPON_DATA_PATH)
                .ToDictionary(data => data.WeaponType, data => data);
            _moduleDatas = Resources
                .LoadAll<ModuleData>(Constants.MODULE_DATA_PATH)
                .ToDictionary(data => data.ModuleType, data => data);
        }

        public ShipData GetShipData(ShipType shipType) 
            => _shipDatas.TryGetValue(shipType, out var data)
                ? data
                : null;

        public WeaponData GetWeaponData(WeaponType weapon)
            => _weaponDatas.TryGetValue(weapon, out var data)
                ? data
                : null;

        public ModuleData GetModuleData(ModuleType module)
            => _moduleDatas.TryGetValue(module, out var data)
                ? data
                : null;

        public WeaponData[] GetAllEnabledWeaponsData() 
            => _weaponDatas.Values.Where(data => data.IsActive).ToArray();

        public ModuleData[] GetAllEnabledModulesData() 
            => _moduleDatas.Values.Where(data => data.IsActive).ToArray();
    }
}