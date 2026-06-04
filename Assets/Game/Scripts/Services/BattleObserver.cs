using System;
using System.Collections.Generic;
using Infrastructure;
using Ships;
using Zenject;

namespace Services
{
    public sealed class BattleObserver : IBattleObserver
    {
        public event Action<IShip> OnWinnerDefined;

        public HashSet<IShip> Ships { get; } = new();
        private bool _isCleaned = true;


        [Inject]
        public BattleObserver(ICleaner cleaner)
        {
            cleaner.AddCleanable(this);
        }

        public void CleanUp() 
            => SceneCleanUp();

        public void SceneCleanUp()
        {
            if (_isCleaned)
                return;
            _isCleaned = true;
            
            foreach (var ship in Ships) 
                ship.OnDied -= DefineWinner;
            Ships.Clear();
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
                {
                    ship.Kill();
                    continue;
                }
                winner = ship;
            }
            OnWinnerDefined?.Invoke(winner);
        }
    }
}