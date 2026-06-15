using UnityEngine;

namespace Ships
{
    public sealed class WeaponView : MonoBehaviour
    {
        [SerializeField] private Transform _barrel;

        public Transform Barrel => _barrel;
    }
}