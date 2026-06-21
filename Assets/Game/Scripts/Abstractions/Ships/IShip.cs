using System;
using Equipment;
using Infrastructure;

namespace Ships
{
    public interface IShip : ISceneCleanable
    {
        event Action<IShip> OnDied;
        
        IHealth Health { get; }
        IWeaponBattery WeaponBattery { get; }
        IShipModules ShipModules { get; }
        string Name { get; }
        void TakeDamage(int damage);
    }
}