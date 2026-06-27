using System;
using Infrastructure;
using UnityEngine;

namespace Ships
{
    public sealed class AmmoView : MonoBehaviour, IUpdatable
    {
        public event Action<Collider2D> OnTriggerEntered;

        private Vector3 _direction;
        private float _speed;

        private void OnTriggerEnter2D(Collider2D other)
            => OnTriggerEntered?.Invoke(other);

        public void OnUpdate(float deltaTime)
        {
            transform.position += _speed * deltaTime * _direction;
        }

        public void Activate(Vector3 position, Quaternion rotation, Vector3 direction, float speed)
        {
            transform.position = position;
            transform.rotation = rotation;
            _direction = direction;
            _speed = speed;
            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            transform.position = Vector3.zero;
            _direction = Vector3.zero;
            _speed = 0;
            gameObject.SetActive(false);
        }
    }
}