using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Infrastructure;

namespace Ships
{
    public interface IShipsInitializer : ISceneCleanable
    {
        Dictionary<OpponentId, IShip> Ships { get; }
        
        UniTask CreateShipsAsync();
    }
}