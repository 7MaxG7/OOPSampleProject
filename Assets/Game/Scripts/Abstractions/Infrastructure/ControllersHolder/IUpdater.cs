namespace Infrastructure
{
    public interface IUpdater : IUpdatable, ICleanable
    {
        void AddUpdatable(IUpdatable updatable);
        void RemoveController(IUpdatable updatable);
    }
}