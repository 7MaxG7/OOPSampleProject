using System.Collections.Generic;

namespace Infrastructure
{
    internal sealed class Updater : IUpdater
    {
        private readonly List<IUpdatable> _updatables = new();
        private readonly List<IUpdatable> _addedUpdatables = new();
        private readonly List<IUpdatable> _removedUpdatables = new();

        public void OnUpdate(float deltaTime)
        {
            UpdateLists();
            foreach (var updatable in _updatables)
                updatable.OnUpdate(deltaTime);
        }

        public void CleanUp()
        {
            _updatables.Clear();
            _addedUpdatables.Clear();
            _removedUpdatables.Clear();
        }

        public void AddUpdatable(IUpdatable updatable)
        {
            if (_removedUpdatables.Remove(updatable) || _updatables.Contains(updatable) || _addedUpdatables.Contains(updatable))
                return;

            _addedUpdatables.Add(updatable);
        }

        public void RemoveUpdatable(IUpdatable updatable)
        {
            if (_addedUpdatables.Remove(updatable) || !_updatables.Contains(updatable) || _removedUpdatables.Contains(updatable))
                return;

            _removedUpdatables.Add(updatable);
        }

        private void UpdateLists()
        {
            foreach (var updatable in _removedUpdatables)
                _updatables.Remove(updatable);
            _removedUpdatables.Clear();

            foreach (var updatable in _addedUpdatables)
                _updatables.Add(updatable);
            _addedUpdatables.Clear();
        }
    }
}
