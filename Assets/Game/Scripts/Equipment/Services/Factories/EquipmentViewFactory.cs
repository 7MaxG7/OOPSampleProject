using Cysharp.Threading.Tasks;
using Infrastructure;
using Ships;
using Zenject;

namespace Equipment
{
    public sealed class EquipmentViewFactory : IEquipmentViewFactory
    {
        private readonly IAssetsInstantiator _assetsInstantiator;
        private readonly IStaticDataService _staticDataService;

        [Inject]
        public EquipmentViewFactory(IAssetsInstantiator assetsInstantiator, IStaticDataService staticDataService)
        {
            _assetsInstantiator = assetsInstantiator;
            _staticDataService = staticDataService;
        }

        public async UniTask<WeaponView> CreateWeaponViewAsync(WeaponType weaponType)
        {
            var config = _staticDataService.GetWeapon(weaponType);
            var weaponView = await _assetsInstantiator.CreateAsync<WeaponView>(config.Prefab);
            weaponView.Init(config.AmmoSpeed);
            return weaponView;
        }

        public async UniTask<ModuleView> CreateModuleViewAsync(ModuleType moduleType)
        {
            var config = _staticDataService.GetModule(moduleType);
            return await _assetsInstantiator.CreateAsync<ModuleView>(config.Prefab);
        }
    }
}