using System;
using Cysharp.Threading.Tasks;
using Equipment.Data;
using Ships;
using UnityEngine;

namespace Equipment
{
    public interface IWeapon : IEquipment
    {
        event Action<IAmmo> OnBulletHit;

        bool IsReady { get; }
        WeaponType WeaponType { get; }

        void Init(IShip owner);
        UniTaskVoid ShootAsync();
        void ReduceCooldown(float deltaTime);
        void TryDealDamage(IAmmo ammo, Collider2D collider);
        void Reload();
    }
}