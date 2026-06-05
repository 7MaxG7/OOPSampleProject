using System;
using System.Collections.Generic;
using Infrastructure.ControllersHolder;
using Ships;

namespace Battle
{
    public interface IBattleObserver : ISceneCleanable
    {
        event Action<IShip> OnWinnerDefined;
        
        HashSet<IShip> Ships { get; }

        void AddShip(IShip ship);
    }
}