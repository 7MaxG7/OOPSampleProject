using System.Collections.Generic;
using System.Linq;
using Equipment;
using Infrastructure;
using Ui;
using UnityEngine;
using Zenject;

namespace UI
{
    public sealed class ShipSetupUIService : IShipSetupUIService
    {
        private readonly IStaticDataService _staticDataService;
        
        private Dictionary<WeaponType, Sprite> _weaponIcons;
        private Dictionary<ModuleType, Sprite> _moduleIcons;

        [Inject]
        public ShipSetupUIService(IStaticDataService staticDataService, ICleaner cleaner)
        {
            _staticDataService = staticDataService;
            cleaner.AddCleanable(this);
        }

        public void CleanUp()
        {
            _weaponIcons?.Clear();
            _moduleIcons?.Clear();
        }

        public void Init()
        {
            _weaponIcons = _staticDataService.GetAllEnabledWeapons()
                .ToDictionary(data => data.WeaponType, data => data.Icon);
            _moduleIcons = _staticDataService.GetAllEnabledModules()
                .ToDictionary(data => data.ModuleType, data => data.Icon);
        }

        public Sprite GetWeaponIcon(WeaponType weaponType)
            => _weaponIcons.GetValueOrDefault(weaponType);

        public Sprite GetModuleIcon(ModuleType moduleType)
            => _moduleIcons.GetValueOrDefault(moduleType);
    }
}