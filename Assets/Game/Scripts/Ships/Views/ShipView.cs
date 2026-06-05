using UnityEngine;

namespace Ships.Views
{
    public sealed class ShipView : MonoBehaviour, IDamageableView
    {
        [SerializeField] private ShieldView _shield;
        [SerializeField] private Transform[] _weaponSlots;
        [SerializeField] private Transform[] _moduleSlots;
        [SerializeField] private Collider2D[] _damageColliders;

        public Transform[] WeaponSlots => _weaponSlots;
        public Transform[] ModuleSlots => _moduleSlots;
        public ShieldView Shield => _shield;
        public Collider2D[] DamageColliders => _damageColliders;
    }
}