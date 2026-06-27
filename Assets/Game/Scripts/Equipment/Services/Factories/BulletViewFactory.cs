using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Infrastructure;
using Ships;
using UnityEngine;
using Utils;
using Zenject;

namespace Equipment
{
    public sealed class BulletViewFactory : IBulletViewFactory
    {
        private readonly IAssetsInstantiator _assetsInstantiator;
        private readonly IStaticDataService _staticDataService;
        
        private readonly Dictionary<WeaponType, BulletViewPool> _bulletPools = new();

        private Transform _bulletsParent;

        [Inject]
        public BulletViewFactory(IAssetsInstantiator assetsInstantiator, IStaticDataService staticDataService, ICleaner cleaner)
        {
            _assetsInstantiator = assetsInstantiator;
            _staticDataService = staticDataService;
            
            cleaner.AddCleanable(this);
        }

        public void CleanUp() 
        {
            foreach (var pool in _bulletPools.Values) 
                pool.Clean();
            _bulletPools.Clear();
        }

        public async UniTask<BulletView> CreateBulletViewAsync(WeaponType weaponType)
            => SpawnBulletFromPool(weaponType) ?? await CreateBulletAsync(weaponType);

        private BulletView SpawnBulletFromPool(WeaponType weaponType)
        {
            if (!_bulletPools.ContainsKey(weaponType)) 
                _bulletPools.Add(weaponType, new BulletViewPool());

            return _bulletPools[weaponType].SpawnBullet();
        }

        private async UniTask<BulletView> CreateBulletAsync(WeaponType weaponType)
        {
            var weaponConfig = _staticDataService.GetWeapon(weaponType);
            var bulletView = await _assetsInstantiator.CreateAsync<BulletView>(weaponConfig.BulletPrefab, GetContent());
            _bulletPools[weaponType].RegisterSpawn(bulletView);
            return bulletView;
        }

        private Transform GetContent()
        {
            if (_bulletsParent == null)
                _bulletsParent = new GameObject(Constants.BULLETS_PARENT_NAME).transform;
            return _bulletsParent;
        }
    }
}
