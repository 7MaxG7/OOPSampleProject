using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Enums;
using Infrastructure;
using Ships;
using Ships.Views;
using UnityEngine;
using Utils;
using Zenject;
using Ammo = Ships.Ammo;

namespace Services
{
    public sealed class AmmoFactory : IAmmoFactory
    {
        private readonly IAssetsProvider _assetsProvider;
        private readonly IStaticDataService _staticDataService;
        
        private readonly Dictionary<WeaponType, IAmmoPool> _ammoPools = new();
        private Transform _ammosParent;


        [Inject]
        public AmmoFactory(IAssetsProvider assetsProvider, IStaticDataService staticDataService, ICleaner cleaner)
        {
            _assetsProvider = assetsProvider;
            _staticDataService = staticDataService;
            cleaner.AddCleanable(this);
        }

        public void CleanUp() 
            => SceneCleanUp();

        public void SceneCleanUp()
        {
            foreach (var pool in _ammoPools.Values) 
                pool.CleanUp();
            _ammoPools.Clear();
        }

        public void PrepareRoot()
        {
            if (_ammosParent == null)
                _ammosParent = new GameObject(Constants.AMMOS_PARENT_NAME).transform;
        }

        public async UniTask<IAmmo> SpawnAmmo(IWeapon weapon)
        {
            var weaponType = weapon.WeaponType;
            if (!_ammoPools.ContainsKey(weaponType)) 
                _ammoPools.Add(weaponType, new AmmoPool());

            return _ammoPools[weaponType].SpawnAmmo(weapon) ?? await CreateAmmo(weapon);
        }

        private async UniTask<IAmmo> CreateAmmo(IWeapon weapon)
        {
            var ammoData = _staticDataService.GetWeaponData(weapon.WeaponType);
            if (ammoData == null)
                return null;
            
            var ammoView = await _assetsProvider.CreateInstanceAsync<AmmoView>(ammoData.AmmoPrefab, _ammosParent);
            var ammo = new Ammo(ammoView);
            _ammoPools[weapon.WeaponType].RegisterAsSpawned(ammo, weapon);
            return ammo;
        }
    }
}