using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Enums;
using Infrastructure;
using Services;
using Ships;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UI.ShipSetup
{
    public sealed class ShipPanelController : ICleanable
    {
        public event Action<OpponentId, int> OnWeaponSelectClick;
        public event Action<OpponentId, int> OnModuleSelectClick;

        private readonly IUiFactory _uiFactory;
        
        private readonly ShipSetupPanelView _shipPanelView;
        private readonly Dictionary<WeaponType, Sprite> _weaponIcons;
        private readonly Dictionary<ModuleType, Sprite> _moduleIcons;
        
        private ShipModel _shipModel;
        
        private readonly List<ShipSlotUiView> _weaponSlots = new();
        private readonly List<ShipSlotUiView> _moduleSlots = new();
        private bool _isCleaned;
        private OpponentId OpponentId => _shipPanelView.OpponentId;
        

        public ShipPanelController(ShipSetupPanelView shipPanelView, IUiFactory uiFactory
            , Dictionary<WeaponType,Sprite> weaponIcons, Dictionary<ModuleType,Sprite> moduleIcons)
        {
            _shipPanelView = shipPanelView;
            _uiFactory = uiFactory;
            _weaponIcons = weaponIcons;
            _moduleIcons = moduleIcons;
        }

        public void CleanUp()
        {
            if (_isCleaned)
                return;
            _isCleaned = true;
            
            _shipModel.OnWeaponChange -= SwitchWeaponIcon;
            _shipModel.OnModuleChange -= SwitchModuleIcon;

            foreach (var slot in _weaponSlots)
            {
                slot.SelectButton.onClick.RemoveAllListeners();
                if (slot != null && slot.gameObject != null)
                    Object.Destroy(slot.gameObject);
            }
            _weaponSlots.Clear();
            
            foreach (var slot in _moduleSlots)
            {
                slot.SelectButton.onClick.RemoveAllListeners();
                if(slot != null && slot.gameObject != null)
                    Object.Destroy(slot.gameObject);
            }
            _moduleSlots.Clear();
        }

        public async UniTask InitAsync(ShipModel shipModel, int weaponsAmount, int modulesAmount)
        {
            _shipModel = shipModel;
            _shipModel.OnWeaponChange += SwitchWeaponIcon;
            _shipModel.OnModuleChange += SwitchModuleIcon;
            await SetupWeaponSlotsAsync(weaponsAmount);
            await SetupModuleSlotsAsync(modulesAmount);
        }

        public Transform GetEquipmentSelectAnchor(EquipmentType type, int index)
        {
            switch (type)
            {
                case EquipmentType.Weapon:
                    return _weaponSlots.FirstOrDefault(slot => slot.Index == index)?.SelectPanelAnchor;
                case EquipmentType.Module:
                    return _moduleSlots.FirstOrDefault(slot => slot.Index == index)?.SelectPanelAnchor;
                default:
                    Debug.LogError($"{this}: No anchor for type {type.ToString()} index {index}");
                    return null;
            }
        }

        private async UniTask SetupWeaponSlotsAsync(int weaponsAmount)
        {
            await CheckSlotsAmountAsync(weaponsAmount, _weaponSlots, _shipPanelView.WeaponSlotsContent, EquipmentType.Weapon);

            for (var slotIndex = 0; slotIndex < weaponsAmount; slotIndex++)
            {
                if (!TryInitSlot(weaponsAmount, _weaponSlots[slotIndex], slotIndex))
                    continue;

                var weaponType = _shipModel.WeaponTypes.TryGetValue(slotIndex, out var type) ? type : WeaponType.None;
                SwitchEquipmentIcon(_weaponSlots[slotIndex].Index, EquipmentType.Weapon, (int)weaponType);
            }
        }

        private async UniTask SetupModuleSlotsAsync(int modulesAmount)
        {
            await CheckSlotsAmountAsync(modulesAmount, _moduleSlots, _shipPanelView.ModuleSlotsContent, EquipmentType.Module);
            
            for (var slotIndex = 0; slotIndex < modulesAmount; slotIndex++)
            {
                if (!TryInitSlot(modulesAmount, _moduleSlots[slotIndex], slotIndex))
                    continue;

                var moduleType = _shipModel.ModuleTypes.TryGetValue(slotIndex, out var type) ? type : ModuleType.None;
                SwitchEquipmentIcon(_moduleSlots[slotIndex].Index, EquipmentType.Module, (int)moduleType);
            }
        }

        private void SwitchWeaponIcon(int slotIndex, WeaponType weaponType) 
            => SwitchEquipmentIcon(slotIndex, EquipmentType.Weapon, (int)weaponType);

        private void SwitchModuleIcon(int slotIndex, ModuleType moduleType) 
            => SwitchEquipmentIcon(slotIndex, EquipmentType.Module, (int)moduleType);

        private void SwitchEquipmentIcon(int slotIndex, EquipmentType type, int typeIndex)
        {
            SlotUiView slot = null;
            Sprite icon = null;
            switch (type)
            {
                case EquipmentType.Weapon:
                    slot = _weaponSlots.FirstOrDefault(item => item.Index == slotIndex);
                    if (_weaponIcons.ContainsKey((WeaponType)typeIndex)) 
                        icon = _weaponIcons[(WeaponType)typeIndex];
                    break;
                case EquipmentType.Module:
                    slot = _moduleSlots.FirstOrDefault(item => item.Index == slotIndex);
                    if (_moduleIcons.ContainsKey((ModuleType)typeIndex)) 
                        icon = _moduleIcons[(ModuleType)typeIndex];
                    break;
                default:
                    Debug.LogError($"{this}: No icon for type {type.ToString()}");
                    break;
            }
            if (slot != null)
                slot.SetIcon(icon);
        }

        private async UniTask CheckSlotsAmountAsync(int amount, List<ShipSlotUiView> currentSlots,
            Transform content, EquipmentType equipmentType)
        {
            currentSlots.ForEach(view => view.gameObject.SetActive(true));
            
            while (currentSlots.Count < amount)
            {
                var slot = await _uiFactory.CreateEquipmentUiSlotAsync(content);
             
                switch (equipmentType)
                {
                    case EquipmentType.Weapon:
                        slot.SelectButton.onClick.AddListener(() => OnWeaponSelectClick?.Invoke(OpponentId, slot.Index));
                        break;
                    case EquipmentType.Module:
                        slot.SelectButton.onClick.AddListener(() => OnModuleSelectClick?.Invoke(OpponentId, slot.Index));
                        break;
                    default:
                        Debug.LogError($"{this}: Unknown equipment {equipmentType} for slot");
                        return;
                }
                currentSlots.Add(slot);
            }
        }

        private bool TryInitSlot(int maxAmount, ShipSlotUiView slot, int index)
        {
            if (index >= maxAmount)
            {
                slot.gameObject.SetActive(false);
                return false;
            }

            slot.Init(OpponentId, index);
            return true;
        }
    }
}
