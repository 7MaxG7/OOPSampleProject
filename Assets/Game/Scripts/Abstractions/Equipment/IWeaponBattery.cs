using System;
using Cysharp.Threading.Tasks;
using Equipment.Data;
using Infrastructure.ControllersHolder;
using Ships;
using UnityEngine;

namespace Equipment
{
    public interface IWeaponBattery : IAbstractEquipments<IWeapon, WeaponType>, IUpdatable
    {
        event Action<WeaponType> OnShoot;
        float ReloadRate { get; }

        void Init(IShip ship);
        UniTask SetEquipmentAsync(int index, WeaponType weaponType);
        void SetSlots(Transform[] weaponSlots);
        void ToggleShooting(bool isActive);
        void SetEquipmentSync(int index, WeaponType weaponType);
    }
}
