namespace Abstractions.Infrastructure
{
    public interface IUpdatable
    {
        public void OnUpdate(float deltaTime);
    }
}