using System;
using Equipment;
using Infrastructure;

namespace Ships
{
    public interface IShip : ISceneCleanable
    {
        event Action<IShip> OnDied;
        
        ShipType ShipType { get; }
        IHealth Health { get; }
        IWeaponBattery WeaponBattery { get; }
        IShipModuleBattery ModuleBattery { get; }
        string Name { get; }
        void TakeDamage(int damage);
        void SetHealth(IHealth health);
        void SetWeapons(IWeaponBattery weapons);
    }
}