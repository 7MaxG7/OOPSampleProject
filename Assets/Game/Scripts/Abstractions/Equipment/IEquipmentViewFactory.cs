using Cysharp.Threading.Tasks;
using Ships;
using UnityEngine;

namespace Equipment
{
    public interface IEquipmentViewFactory
    {
        UniTask<WeaponView> CreateWeaponViewAsync(WeaponType weaponType, Transform parent);
        UniTask<ModuleView> CreateModuleViewAsync(ModuleType moduleType, Transform parent);
    }
}