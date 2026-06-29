namespace Infrastructure
{
    public interface ICleaner : ICleanable
    {
        void AddCleanable(ICleanable cleanable);
        void AddCleanable(ICleanable cleanable, int priority);
        void RemoveCleanable(ICleanable cleanable);
        void SceneCleanUp();
    }
}