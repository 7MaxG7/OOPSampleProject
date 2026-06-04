using System;
using Infrastructure;
using Ships.Views;

namespace Ships
{
    public interface IShip : ISceneCleanable
    {
        event Action<IShip> OnDied;
        
        IHealth Health { get; }
        IWeaponBattery WeaponBattery { get; }
        IShipModules ShipModules { get; }
        string Name { get; }
        ShipView ShipView { get; }

        void PrepareToBattle();
        void TakeDamage(int damage);
        void Kill();
    }
}