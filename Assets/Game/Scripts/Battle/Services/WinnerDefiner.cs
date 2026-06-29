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

        private readonly HashSet<IShip> _ships = new();
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
            
            foreach (var ship in _ships) 
                ship.OnDied -= DefineWinner;
            _ships.Clear();
            _isCleaned = true;
        }

        public void AddShip(IShip ship)
        {
            _isCleaned = false;
            if (_ships.Add(ship))
                ship.OnDied += DefineWinner;
        }

        private void DefineWinner(IShip looser)
        {
            IShip winner = null;
            foreach (var ship in _ships)
            {
                if (ship == looser)
                    continue;
                winner = ship;
            }
            OnWinnerDefined?.Invoke(winner);
        }
    }
}