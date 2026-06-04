using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Abstractions.Infrastructure;
using Abstractions.Ships;
using Enums;

namespace Abstractions
{
    public interface IShipsInitializer : ISceneCleanable
    {
        Dictionary<OpponentId, IShip> Ships { get; }
        
        UniTask PrepareShipsAsync();
    }
}