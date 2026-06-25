using Cysharp.Threading.Tasks;
using Infrastructure;

namespace Ships
{
    public interface IShipsViewInitializer : ISceneCleanable
    {
        UniTask CreateShipsViewsAsync();
    }
}