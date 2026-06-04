using Cysharp.Threading.Tasks;
using Enums;
using Ships;
using Ships.Views;
using UnityEngine;
using Zenject;

namespace Services
{
    public sealed class ModuleFactory : IModuleFactory
    {
        private readonly IStaticDataService _staticDataService;
        private readonly IAssetsProvider _assetsProvider;


        [Inject]
        public ModuleFactory(IStaticDataService staticDataService, IAssetsProvider assetsProvider)
        {
            _staticDataService = staticDataService;
            _assetsProvider = assetsProvider;
        }
        
        public async UniTask<IModule> CreateEquipment(ModuleType moduleType, Transform parent)
        {
            var moduleData = _staticDataService.GetModuleData(moduleType);
            var module = new Module(moduleData.EffectType, moduleData.MathType, moduleData.Value, moduleData.ModuleType);
            var view = await _assetsProvider.CreateInstanceAsync<ModuleView>(moduleData.Prefab, parent);
            module.SetView(view);
            return module;
        }
    }
}