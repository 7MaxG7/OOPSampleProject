using System.Collections.Generic;


namespace Infrastructure
{
    internal sealed class Updater : IUpdater
    {
        private readonly List<IUpdatable> _updatables = new();

        public void OnUpdate(float deltaTime)
        {
            foreach (var updatable in _updatables)
                updatable.OnUpdate(deltaTime);
        }

        public void AddUpdatable(IUpdatable updatable)
        {
            _updatables.Add(updatable);
        }

        public void RemoveController(IUpdatable updatable)
        {
            _updatables.Remove(updatable);
        }

        public void CleanUp()
        {
            _updatables.Clear();
        }
    }
}