using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Equipment;
using Ships;
using UnityEngine;
using Utils;

namespace UI.ShipSetup
{
    public sealed class ShipSetupPanelUIModel
    {
        public Dictionary<int, AsyncReactiveProperty<WeaponType>> WeaponSlots { get; } = new();
        public Dictionary<int, AsyncReactiveProperty<ModuleType>> ModuleSlots { get; } = new();

        public void SetWeapon(IShip ship, int slotIndex, IWeapon weapon)
        {
            if (!WeaponSlots.TryGetValue(slotIndex, out var slotWeapon))
            {
                Debug.LogError($"Cannot get weapon slot {slotIndex} in UI model");
                return;
            }
            
            slotWeapon.Update(weapon.WeaponType);
        }

        public void SetModule(IShip ship, int slotIndex, IModule module)
        {
            if (!ModuleSlots.TryGetValue(slotIndex, out var slotModule))
            {
                Debug.LogError($"Cannot get module slot {slotIndex} in UI model");
                return;
            }
            
            slotModule.Update(module.ModuleType);
        }
    }
}