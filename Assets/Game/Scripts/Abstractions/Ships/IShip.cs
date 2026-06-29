using System;
using Equipment;

namespace Ships
{
    public interface IShip
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
        void Clean();
    }
}