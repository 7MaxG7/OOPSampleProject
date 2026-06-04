using UnityEngine;

namespace Ships.Views
{
    public sealed class WeaponView : MonoBehaviour
    {
        [SerializeField] private Transform _barrel;

        public Transform Barrel => _barrel;
    }
}