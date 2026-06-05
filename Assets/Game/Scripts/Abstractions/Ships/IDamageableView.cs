using UnityEngine;

namespace Ships
{
    public interface IDamageableView
    {
        Collider2D[] DamageColliders { get; }
    }
}