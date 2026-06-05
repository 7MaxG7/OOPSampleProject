using System.Collections.Generic;
using System.Linq;
using Equipment;
using Equipment.Data;
using Ships;
using Ships.Data;
using UnityEngine;
using Utils;

namespace Infrastructure
{
    public sealed class StaticDataService : IStaticDataService
    {
        private Dictionary<ShipType, ShipConfig> _shipConfigs;
        private Dictionary<WeaponType, WeaponConfig> _weaponConfigs;
        private Dictionary<ModuleType, ModuleConfig> _moduleConfigs;

        public void Init()
        {
            _shipConfigs = Resources
                .LoadAll<ShipConfig>(Constants.SHIP_DATA_PATH)
                .ToDictionary(data => data.ShipType, data => data);
            _weaponConfigs = Resources
                .LoadAll<WeaponConfig>(Constants.WEAPON_DATA_PATH)
                .ToDictionary(data => data.WeaponType, data => data);
            _moduleConfigs = Resources
                .LoadAll<ModuleConfig>(Constants.MODULE_DATA_PATH)
                .ToDictionary(data => data.ModuleType, data => data);
        }

        public ShipConfig GetShip(ShipType shipType) 
            => _shipConfigs.GetValueOrDefault(shipType);

        public WeaponConfig GetWeapon(WeaponType weapon)
            => _weaponConfigs.GetValueOrDefault(weapon);

        public ModuleConfig GetModule(ModuleType module)
            => _moduleConfigs.GetValueOrDefault(module);

        public WeaponConfig[] GetAllEnabledWeapons() 
            => _weaponConfigs.Values.Where(data => data.IsActive).ToArray();

        public ModuleConfig[] GetAllEnabledModules() 
            => _moduleConfigs.Values.Where(data => data.IsActive).ToArray();
    }
}