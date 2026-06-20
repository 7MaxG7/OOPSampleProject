using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Equipment;
using UnityEngine;
using Utils;

namespace UI.Ship
{
    public sealed class ShipSetupPanelUIModel
    {
        public Dictionary<int, AsyncReactiveProperty<WeaponType>> WeaponSlots { get; } = new();
        public Dictionary<int, AsyncReactiveProperty<ModuleType>> ModuleSlots { get; } = new();

        public void SetWeapon(int slotIndex, WeaponType weaponType)
        {
            if (!WeaponSlots.TryGetValue(slotIndex, out var slotWeapon))
            {
                Debug.LogError($"Cannot get weapon slot {slotIndex} in UI model");
                return;
            }
            
            slotWeapon.Update(weaponType);
        }

        public void SetModule(int slotIndex, ModuleType moduleType)
        {
            if (!ModuleSlots.TryGetValue(slotIndex, out var slotModule))
            {
                Debug.LogError($"Cannot get module slot {slotIndex} in UI model");
                return;
            }
            
            slotModule.Update(moduleType);
        }
    }
}