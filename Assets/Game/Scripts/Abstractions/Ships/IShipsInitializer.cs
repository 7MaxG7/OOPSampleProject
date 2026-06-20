using Cysharp.Threading.Tasks;
using Infrastructure;

namespace Ships
{
    public interface IShipsInitializer : ISceneCleanable
    {
        UniTask CreateShipsAsync();
    }
}