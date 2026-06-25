using Battle;
using Infrastructure;
using Zenject;

namespace Equipment
{
    public sealed class EquipmentFactory : IWeaponFactory, IModuleFactory
    {
        private readonly IStaticDataService _staticDataService;
        private readonly IDamageHandler _damageHandler;

        [Inject]
        public EquipmentFactory(IStaticDataService staticDataService, IDamageHandler damageHandler)
        {
            _staticDataService = staticDataService;
            _damageHandler = damageHandler;
        }
        
        public IWeapon CreateEquipment(WeaponType weaponType)
        {
            var config = _staticDataService.GetWeapon(weaponType);
            return new Weapon(config.Cooldown, config.Damage, weaponType, _damageHandler);
        }

        public IModule CreateEquipment(ModuleType moduleType)
        {
            var config = _staticDataService.GetModule(moduleType);
            return new Module(config.BuffParamType, config.BuffRelativenessType, config.Value, config.ModuleType);
        }
    }
}