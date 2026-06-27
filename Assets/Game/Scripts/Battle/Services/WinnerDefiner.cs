using System;
using System.Collections.Generic;
using Infrastructure;
using Ships;
using Zenject;

namespace Battle
{
    public sealed class WinnerDefiner : IWinnerDefiner
    {
        public event Action<IShip> OnWinnerDefined;

        public HashSet<IShip> Ships { get; } = new();
        private bool _isCleaned = true;

        [Inject]
        public WinnerDefiner(ICleaner cleaner)
        {
            cleaner.AddCleanable(this);
        }

        public void CleanUp() 
        {
            if (_isCleaned)
                return;
            
            foreach (var ship in Ships) 
                ship.OnDied -= DefineWinner;
            Ships.Clear();
            _isCleaned = true;
        }

        public void AddShip(IShip ship)
        {
            _isCleaned = false;
            if (Ships.Add(ship))
                ship.OnDied += DefineWinner;
        }

        private void DefineWinner(IShip looser)
        {
            IShip winner = null;
            foreach (var ship in Ships)
            {
                if (ship == looser)
                    continue;
                winner = ship;
            }
            OnWinnerDefined?.Invoke(winner);
        }
    }
}