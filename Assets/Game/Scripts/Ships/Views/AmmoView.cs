using System;
using UnityEngine;

namespace Ships.Views
{
    public sealed class AmmoView : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidbody;
        
        public event Action<Collider2D> OnTriggerEntered;

        public Rigidbody2D Rigidbody => _rigidbody;
  
        
        private void OnTriggerEnter2D(Collider2D other) 
            => OnTriggerEntered?.Invoke(other);
    }
}