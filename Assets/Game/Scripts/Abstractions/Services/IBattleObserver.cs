using System;
using System.Collections.Generic;
using Infrastructure;
using Ships;

namespace Services
{
    public interface IBattleObserver : ISceneCleanable
    {
        event Action<IShip> OnWinnerDefined;
        
        HashSet<IShip> Ships { get; }

        void AddShip(IShip ship);
    }
}