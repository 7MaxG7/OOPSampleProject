namespace Infrastructure.ControllersHolder
{
    public interface IUpdater : IUpdatable, ICleanable
    {
        void AddUpdatable(IUpdatable updatable);
        void RemoveController(IUpdatable updatable);
    }
}