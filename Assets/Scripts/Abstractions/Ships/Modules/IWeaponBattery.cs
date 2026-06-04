using System;
using Cysharp.Threading.Tasks;
using Abstractions.Infrastructure;
using Enums;
using UnityEngine;

namespace Abstractions.Ships
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
