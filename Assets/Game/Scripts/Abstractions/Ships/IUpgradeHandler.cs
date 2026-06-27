using Equipment;

namespace Ships
{
    public interface IUpgradeHandler
    {
        void Upgrade(IShip ship, IModule module);
        void Downgrade(IShip ship, IModule module);
    }
}