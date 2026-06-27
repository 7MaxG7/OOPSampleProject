using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Infrastructure;
using Ships;
using UnityEngine;
using Utils;
using Zenject;

namespace Equipment
{
    public sealed class AmmoViewFactory : IAmmoViewFactory
    {
        private readonly IAssetsInstantiator _assetsInstantiator;
        private readonly IStaticDataService _staticDataService;
        
        private readonly Dictionary<WeaponType, AmmoViewPool> _ammoPools = new();

        private Transform _ammosParent;

        [Inject]
        public AmmoViewFactory(IAssetsInstantiator assetsInstantiator, IStaticDataService staticDataService, ICleaner cleaner)
        {
            _assetsInstantiator = assetsInstantiator;
            _staticDataService = staticDataService;
            
            cleaner.AddCleanable(this);
        }

        public void CleanUp() 
        {
            foreach (var pool in _ammoPools.Values) 
                pool.Clean();
            _ammoPools.Clear();
        }

        public async UniTask<AmmoView> CreateAmmoViewAsync(WeaponType weaponType)
            => SpawnAmmoFromPool(weaponType) ?? await CreateAmmoAsync(weaponType);

        private AmmoView SpawnAmmoFromPool(WeaponType weaponType)
        {
            if (!_ammoPools.ContainsKey(weaponType)) 
                _ammoPools.Add(weaponType, new AmmoViewPool());

            return _ammoPools[weaponType].SpawnAmmo();
        }

        private async UniTask<AmmoView> CreateAmmoAsync(WeaponType weaponType)
        {
            var weaponConfig = _staticDataService.GetWeapon(weaponType);
            var ammoView = await _assetsInstantiator.CreateAsync<AmmoView>(weaponConfig.AmmoPrefab, GetContent());
            _ammoPools[weaponType].RegisterSpawn(ammoView);
            return ammoView;
        }

        private Transform GetContent()
        {
            if (_ammosParent == null)
                _ammosParent = new GameObject(Constants.AMMOS_PARENT_NAME).transform;
            return _ammosParent;
        }
    }
}
