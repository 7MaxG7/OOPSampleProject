using Abstractions.Infrastructure;
using UnityEngine;

namespace Abstractions.Ships
{
    public interface IAmmo : ICleanable
    {
        Rigidbody2D Rigidbody { get; }

        void Activate(Transform startPosition, IWeapon shooter);
        void Deactivate();
    }
}