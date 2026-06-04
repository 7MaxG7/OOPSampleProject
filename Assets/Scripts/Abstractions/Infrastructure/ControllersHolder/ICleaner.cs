namespace Abstractions.Infrastructure
{
    public interface ICleaner : ICleanable
    {
        void AddCleanable(ICleanable cleanable);
        void RemoveCleanable(ICleanable cleanable);
        void SceneCleanUp();
    }
}