using System.Collections.Generic;
using UnityEngine;

namespace Ships
{
    public sealed class ShipView : MonoBehaviour, IDamageableView
    {
        [SerializeField] private ShieldView _shield;
        [SerializeField] private Transform[] _weaponSlots;
        [SerializeField] private Transform[] _moduleSlots;
        [SerializeField] private Collider2D[] _damageColliders;

        public ShieldView Shield => _shield;
        public Collider2D[] DamageColliders => _damageColliders;

        private readonly Dictionary<int, WeaponView> _weaponViews = new();
        private readonly Dictionary<int, ModuleView> _moduleViews = new();

        public void Init(int weaponSlotsAmount, int moduleSlotsAmount)
        {
            UpdateSlotsVisibility(_weaponSlots, weaponSlotsAmount);
            UpdateSlotsVisibility(_moduleSlots, moduleSlotsAmount);
        }

        public void SetWeaponView(int slotIndex, WeaponView weaponView)
        {
            if (slotIndex < _weaponSlots.Length)
                weaponView.transform.SetParent(_weaponSlots[slotIndex], false);
            else
                Debug.LogError($"{this}: Cannot get weapon slot {slotIndex}!");

            _weaponViews[slotIndex] = weaponView;
        }

        public void SetModuleView(int slotIndex, ModuleView moduleView)
        {
            if (slotIndex < _moduleSlots.Length)
                moduleView.transform.SetParent(_moduleSlots[slotIndex], false);
            else
                Debug.LogError($"{this}: Cannot get module slot {slotIndex}!");

            _moduleViews[slotIndex] = moduleView;
        }

        public void UnequipWeaponView(int slotIndex)
        {
            if (!_weaponViews.Remove(slotIndex, out var weaponView))
                return;

            Destroy(weaponView.gameObject);
        }

        public void UnequipModuleView(int slotIndex)
        {
            if (!_moduleViews.Remove(slotIndex, out var moduleView))
                return;

            Destroy(moduleView.gameObject);
        }

        private void UpdateSlotsVisibility(Transform[] slots, int activeSlotsAmount)
        {
            for (var i = 0; i < slots.Length; i++)
                slots[i].gameObject.SetActive(i < activeSlotsAmount);
        }
    }
}