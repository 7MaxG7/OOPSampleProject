using Infrastructure;
using Ships;
using UnityEngine;

namespace Equipment
{
    public interface IAmmo : ICleanable
    {
        AmmoView AmmoView { get; }

        void Activate(Vector3 position, Quaternion rotation, Vector3 direction, float speed, IWeapon shooter);
        void Deactivate();
    }
}
