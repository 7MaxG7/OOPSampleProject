using System.Collections.Generic;
using DG.Tweening;

namespace Infrastructure
{
    internal sealed class Cleaner : ICleaner
    {
        private const int DEFAULT_PRIORITY = 100;
        private readonly List<(ICleanable service, int priority)> _cleanables = new();
        private readonly List<(ICleanable service, int priority)> _sceneCleanables = new();
        private bool _areSorted;

        public void AddCleanable(ICleanable cleanable)
            => AddCleanable(cleanable, DEFAULT_PRIORITY);

        public void AddCleanable(ICleanable cleanable, int priority)
        {
            _cleanables.Add((cleanable, priority));
            if (cleanable is ISceneCleanable sceneCleanable) 
                _sceneCleanables.Add((sceneCleanable, priority));
            _areSorted = false;
        }

        public void RemoveCleanable(ICleanable cleanable)
        {
            _cleanables.RemoveAll(data => data.service == cleanable);
            if (cleanable is ISceneCleanable sceneCleanable) 
                _sceneCleanables.RemoveAll(data => data.service == cleanable);
        }

        public void SceneCleanUp()
        {
            SortCleanables();
            foreach (var cleanable in _sceneCleanables)
                cleanable.service.CleanUp();
            _areSorted = true;
        }

        public void CleanUp()
        {
            SortCleanables();
            foreach (var cleanable in _cleanables)
                cleanable.service.CleanUp();
            _cleanables.Clear();
            DOTween.Clear();
        }

        private void SortCleanables()
        {
            if (_areSorted)
                return;
            
            _cleanables.Sort((a, b) => a.priority.CompareTo(b.priority));
            _sceneCleanables.Sort((a, b) => a.priority.CompareTo(b.priority));
            _areSorted = true;
        }
    }
}