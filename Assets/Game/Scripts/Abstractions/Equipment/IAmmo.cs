using Infrastructure;
using UnityEngine;

namespace Equipment
{
    public interface IAmmo : ICleanable
    {
        Rigidbody2D Rigidbody { get; }

        void Activate(Transform startPosition, IWeapon shooter);
        void Deactivate();
    }
}