using Battle;
using Zenject;

namespace Ships
{
    public sealed class ShipsInitializer : IShipsInitializer
    {
        private readonly IShipsFactory _shipsFactory;
        private readonly IShipConfigurator _shipConfigurator;
        private readonly IWinnerDefiner _winnerDefiner;

        [Inject]
        public ShipsInitializer(IShipsFactory shipsFactory, IShipConfigurator shipConfigurator, IWinnerDefiner winnerDefiner)
        {
            _shipsFactory = shipsFactory;
            _shipConfigurator = shipConfigurator;
            _winnerDefiner = winnerDefiner;
        }

        public void CreateShips()
        {
            foreach (var (opponentId, configuration) in _shipConfigurator.ShipConfigurations)
            {
                var ship = _shipsFactory.CreateShip(configuration.ShipType);
                foreach (var (slotIndex, weaponType) in configuration.WeaponTypes)
                    ship.WeaponBattery.SetEquipment(slotIndex, weaponType);
                foreach (var (slotIndex, moduleType) in configuration.ModuleTypes)
                    ship.ModuleBattery.SetEquipment(slotIndex, moduleType);
                _shipConfigurator.RegisterShip(opponentId, ship);
                
                _winnerDefiner.AddShip(ship);
            }
        }
    }
}