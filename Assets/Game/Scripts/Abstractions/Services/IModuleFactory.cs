using Enums;
using Ships;

namespace Services
{
    public interface IModuleFactory : IEquipmentFactory<IModule, ModuleType>
    {
    }
}