using Cysharp.Threading.Tasks;
using Equipment;
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

        private IEquipmentViewFactory _equipmentViewFactory;
        private readonly Dictionary<int, WeaponView> _weaponViews = new();
        private readonly Dictionary<int, ModuleView> _moduleViews = new();

        public void Init(int weaponSlotsAmount, int moduleSlotsAmount, IEquipmentViewFactory equipmentViewFactory)
        {
            _equipmentViewFactory = equipmentViewFactory;

            UpdateSlotsVisibility(_weaponSlots, weaponSlotsAmount);
            UpdateSlotsVisibility(_moduleSlots, moduleSlotsAmount);
        }

        public async UniTask<WeaponView> CreateWeaponViewAsync(int slotIndex, WeaponType weaponType)
        {
            if (slotIndex >= _weaponSlots.Length)
                return null;

            var weaponView = await _equipmentViewFactory.CreateWeaponViewAsync(weaponType, _weaponSlots[slotIndex]);
            _weaponViews[slotIndex] = weaponView;
            return weaponView;
        }

        public void UnequipWeaponView(int slotIndex)
        {
            if (!_weaponViews.Remove(slotIndex, out var weaponView))
                return;

            Destroy(weaponView.gameObject);
        }

        public async UniTask<ModuleView> CreateModuleViewAsync(int slotIndex, ModuleType moduleType)
        {
            if (slotIndex >= _moduleSlots.Length)
                return null;

            var moduleView = await _equipmentViewFactory.CreateModuleViewAsync(moduleType, _moduleSlots[slotIndex]);
            _moduleViews[slotIndex] = moduleView;
            return moduleView;
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