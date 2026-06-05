using Equipment;

namespace Ships
{
    public interface IDowngradable<out T>
    {
        T Downgrade(IModule module);
    }
}