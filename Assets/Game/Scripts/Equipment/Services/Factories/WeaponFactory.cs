using Battle;
using Cysharp.Threading.Tasks;
using Infrastructure;
using Ships;
using UnityEngine;
using Zenject;

namespace Equipment
{
    public sealed class WeaponFactory : IWeaponFactory
    {
        private readonly IStaticDataService _staticDataService;
        private readonly IAssetsInstantiator _instantiator;
        private readonly IAmmoFactory _ammoFactory;
        private readonly IDamageHandler _damageHandler;

        [Inject]
        public WeaponFactory(IStaticDataService staticDataService, IAssetsInstantiator instantiator, IAmmoFactory ammoFactory
            , IDamageHandler damageHandler)
        {
            _staticDataService = staticDataService;
            _instantiator = instantiator;
            _ammoFactory = ammoFactory;
            _damageHandler = damageHandler;
        }
        
        public async UniTask<IWeapon> CreateEquipment(WeaponType weaponType, Transform parent)
        {
            var config = _staticDataService.GetWeapon(weaponType);
            var weapon = new Weapon(config.Cooldown, config.Damage, config.AmmoSpeed, weaponType, _ammoFactory
                , _damageHandler);
            var view = await _instantiator.CreateAsync<WeaponView>(config.Prefab, parent);
            weapon.SetView(view);
            return weapon;
        }
    }
}
