using System;
using Infrastructure;
using Ships;

namespace Battle
{
    public interface IWinnerDefiner : ISceneCleanable
    {
        event Action<IShip> OnWinnerDefined;

        void AddShip(IShip ship);
    }
}