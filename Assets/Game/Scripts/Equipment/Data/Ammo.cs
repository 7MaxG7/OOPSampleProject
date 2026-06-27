using Ships;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Equipment
{
    public sealed class Ammo : IAmmo
    {
        public AmmoView AmmoView { get; }

        private IWeapon _shooter;

        public Ammo(AmmoView view)
        {
            AmmoView = view;
            AmmoView.OnTriggerEntered += HandleCollision;
        }

        public void CleanUp()
        {
            AmmoView.OnTriggerEntered -= HandleCollision;
            if (AmmoView != null && AmmoView.gameObject != null)
                Object.Destroy(AmmoView.gameObject);
        }

        public void Activate(Vector3 position, Quaternion rotation, Vector3 direction, float speed, IWeapon shooter)
        {
            _shooter = shooter;
            AmmoView.Activate(position, rotation, direction, speed);
        }

        public void Deactivate()
        {
            AmmoView.Deactivate();
        }

        private void HandleCollision(Collider2D collider)
        {
            _shooter.TryDealDamage(this, collider);
        }
    }
}
