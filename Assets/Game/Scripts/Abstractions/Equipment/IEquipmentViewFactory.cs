using Cysharp.Threading.Tasks;
using Ships;

namespace Equipment
{
    public interface IEquipmentViewFactory
    {
        UniTask<WeaponView> CreateWeaponViewAsync(WeaponType weaponType);
        UniTask<ModuleView> CreateModuleViewAsync(ModuleType moduleType);
    }
}