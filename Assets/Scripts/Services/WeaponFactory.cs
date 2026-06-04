using Cysharp.Threading.Tasks;
using Abstractions.Services;
using Abstractions.Ships;
using Enums;
using Ships.Modules;
using Ships.Views;
using UnityEngine;
using Zenject;

namespace Services
{
    public sealed class WeaponFactory : IWeaponFactory
    {
        private readonly IStaticDataService _staticDataService;
        private readonly IAssetsProvider _assetsProvider;
        private readonly IAmmoFactory _ammoFactory;
        private readonly IDamageHandler _damageHandler;


        [Inject]
        public WeaponFactory(IStaticDataService staticDataService, IAssetsProvider assetsProvider, IAmmoFactory ammoFactory
            , IDamageHandler damageHandler)
        {
            _staticDataService = staticDataService;
            _assetsProvider = assetsProvider;
            _ammoFactory = ammoFactory;
            _damageHandler = damageHandler;
        }
        
        public async UniTask<IWeapon> CreateEquipment(WeaponType weaponType, Transform parent)
        {
            var weaponData = _staticDataService.GetWeaponData(weaponType);
            var weapon = new Weapon(weaponData.Cooldown, weaponData.Damage, weaponData.AmmoSpeed, weaponType, _ammoFactory
                , _damageHandler);
            var view = await _assetsProvider.CreateInstanceAsync<WeaponView>(weaponData.Prefab, parent);
            weapon.SetView(view);
            return weapon;
        }
    }
}
