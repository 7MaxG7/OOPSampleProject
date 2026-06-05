using Ships;
using Ships.Views;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Equipment.Data
{
    public sealed class Ammo : IAmmo
    {
        public Rigidbody2D Rigidbody => _ammoView.Rigidbody;

        private Transform Transform => _ammoView.transform;
        private readonly AmmoView _ammoView;
        private IWeapon _shooter;

        public Ammo(AmmoView view)
        {
            _ammoView = view;
            _ammoView.OnTriggerEntered += HandleCollision;
        }

        public void CleanUp()
        {
            _ammoView.OnTriggerEntered -= HandleCollision;
            if (_ammoView != null && _ammoView.gameObject != null)
                Object.Destroy(_ammoView.gameObject);
        }

        public void Activate(Transform start, IWeapon shooter)
        {
            _shooter = shooter;
            _ammoView.gameObject.SetActive(true);
            Transform.position = start.position;
            Transform.rotation = start.rotation;
        }

        public void Deactivate()
        {
            Transform.position = Vector3.zero;
            Rigidbody.velocity = Vector2.zero;
            _ammoView.gameObject.SetActive(false);
        }

        private void HandleCollision(Collider2D collider)
        {
            if (collider.TryGetComponent<IDamagableView>(out var damageTaker))
                _shooter.TryDealDamage(this, damageTaker);
        }
    }
}