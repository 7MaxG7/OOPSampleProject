using System.Collections.Generic;
using DG.Tweening;

namespace Infrastructure
{
    internal sealed class Cleaner : ICleaner
    {
        private readonly List<ICleanable> _cleanables = new();
        private readonly List<ISceneCleanable> _sceneCleanables = new();
        

        public void AddCleanable(ICleanable cleanable)
        {
            _cleanables.Add(cleanable);
            if (cleanable is ISceneCleanable sceneCleanable) 
                _sceneCleanables.Add(sceneCleanable);
        }

        public void RemoveCleanable(ICleanable cleanable)
        {
            _cleanables.Remove(cleanable);
            if (cleanable is ISceneCleanable sceneCleanable) 
                _sceneCleanables.Remove(sceneCleanable);
        }

        public void SceneCleanUp()
        {
            foreach (var cleanable in _sceneCleanables)
                cleanable.SceneCleanUp();
        }

        public void CleanUp()
        {
            foreach (var cleanable in _cleanables)
                cleanable.CleanUp();
            _cleanables.Clear();
            DOTween.Clear();
        }
    }
}