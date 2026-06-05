using Equipment.Data;

namespace Equipment
{
    public interface IModuleFactory : IEquipmentFactory<IModule, ModuleType>
    {
    }
}