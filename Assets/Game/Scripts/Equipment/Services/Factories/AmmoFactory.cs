using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Infrastructure;
using Ships;
using UnityEngine;
using Utils;
using Zenject;

namespace Equipment
{
    public sealed class AmmoFactory : IAmmoFactory
    {
        private readonly IAssetsInstantiator _instantiator;
        private readonly IStaticDataService _staticDataService;
        
        private readonly Dictionary<WeaponType, IAmmoPool> _ammoPools = new();
        private Transform _ammosParent;

        [Inject]
        public AmmoFactory(IAssetsInstantiator instantiator, IStaticDataService staticDataService, ICleaner cleaner)
        {
            _instantiator = instantiator;
            _staticDataService = staticDataService;
            cleaner.AddCleanable(this);
        }

        public void CleanUp() 
        {
            foreach (var pool in _ammoPools.Values) 
                pool.CleanUp();
            _ammoPools.Clear();
        }

        public async UniTask<IAmmo> SpawnAmmoAsync(IWeapon weapon)
        {
            var weaponType = weapon.WeaponType;
            if (!_ammoPools.ContainsKey(weaponType)) 
                _ammoPools.Add(weaponType, new AmmoPool());

            return _ammoPools[weaponType].SpawnAmmo(weapon) ?? await CreateAmmoAsync(weapon);
        }

        private async UniTask<IAmmo> CreateAmmoAsync(IWeapon weapon)
        {
            var weaponConfig = _staticDataService.GetWeapon(weapon.WeaponType);
            if (weaponConfig == null)
                return null;
            
            var ammoView = await _instantiator.CreateAsync<AmmoView>(weaponConfig.AmmoPrefab, GetContent());
            var ammo = new Ammo(ammoView);
            _ammoPools[weapon.WeaponType].RegisterSpawn(ammo, weapon);
            return ammo;
        }

        private Transform GetContent()
        {
            if (_ammosParent == null)
                _ammosParent = new GameObject(Constants.AMMOS_PARENT_NAME).transform;
            return _ammosParent;
        }
    }
}