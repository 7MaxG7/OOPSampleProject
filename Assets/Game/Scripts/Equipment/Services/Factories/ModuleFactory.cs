using Cysharp.Threading.Tasks;
using Infrastructure;
using Ships;
using UnityEngine;
using Zenject;

namespace Equipment
{
    public sealed class ModuleFactory : IModuleFactory
    {
        private readonly IStaticDataService _staticDataService;
        private readonly IAssetsInstantiator _instantiator;

        [Inject]
        public ModuleFactory(IStaticDataService staticDataService, IAssetsInstantiator instantiator)
        {
            _staticDataService = staticDataService;
            _instantiator = instantiator;
        }
        
        public async UniTask<IModule> CreateEquipment(ModuleType moduleType, Transform parent)
        {
            var config = _staticDataService.GetModule(moduleType);
            var module = new Module(config.BuffParamType, config.BuffRelativenessType, config.Value, config.ModuleType);
            var view = await _instantiator.CreateAsync<ModuleView>(config.Prefab, parent);
            module.SetView(view);
            return module;
        }
    }
}