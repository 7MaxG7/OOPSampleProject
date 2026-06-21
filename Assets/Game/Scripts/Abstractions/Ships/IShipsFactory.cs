namespace Ships
{
    public interface IShipsFactory
    {
        IShip CreateShip(ShipType shipType);
    }
}