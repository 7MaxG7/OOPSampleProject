using Cysharp.Threading.Tasks;
using Equipment.Data;
using Infrastructure;
using Ships.Views;
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
            var moduleData = _staticDataService.GetModule(moduleType);
            var module = new Module(moduleData.BuffParamType, moduleData.BuffRelativenessType, moduleData.Value, moduleData.ModuleType);
            var view = await _instantiator.CreateAsync<ModuleView>(moduleData.Prefab, parent);
            module.SetView(view);
            return module;
        }
    }
}