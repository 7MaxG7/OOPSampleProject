using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Infrastructure.ControllersHolder;
using Ships.Data;

namespace Ships
{
    public interface IShipsInitializer : ISceneCleanable
    {
        Dictionary<OpponentId, IShip> Ships { get; }
        
        UniTask PrepareShipsAsync();
    }
}