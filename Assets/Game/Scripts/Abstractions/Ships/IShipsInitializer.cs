using Infrastructure;

namespace Ships
{
    public interface IShipsInitializer : ISceneCleanable
    {
        void CreateShips();
    }
}