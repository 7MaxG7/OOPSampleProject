using Equipment;

namespace Ships
{
    public interface IShipUpgrader
    {
        void Upgrade(IShip ship, IModule module);
        void Downgrade(IShip ship, IModule module);
    }
}