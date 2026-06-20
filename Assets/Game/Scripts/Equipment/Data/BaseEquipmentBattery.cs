using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Equipment
{

    public abstract class BaseEquipmentBattery<TEquipment, TEquipType> : IEquipmentBattery<TEquipment, TEquipType>
        where TEquipment : IEquipment where TEquipType : Enum
    {
        public event Action<int, TEquipType> OnEquipmentChanged;

        public int MaxEquipmentsAmount { get; }
        public IEquipmentFactory<TEquipment, TEquipType> EquipmentsFactory { get; }
        public Dictionary<int, TEquipment> Equipments { get; } = new();
        public Dictionary<int, Transform> Slots { get; } = new();

        protected BaseEquipmentBattery(int amount, IEquipmentFactory<TEquipment, TEquipType> equipmentFactory)
        {
            MaxEquipmentsAmount = amount;
            EquipmentsFactory = equipmentFactory;
        }

        protected BaseEquipmentBattery(IEquipmentBattery<TEquipment, TEquipType> baseEquipmentBattery)
        {
            MaxEquipmentsAmount = baseEquipmentBattery.MaxEquipmentsAmount;
            Equipments = baseEquipmentBattery.Equipments;
            EquipmentsFactory = baseEquipmentBattery.EquipmentsFactory;
            Slots = baseEquipmentBattery.Slots;
        }
        
        public void SetSlots(Transform[] slots)
        {
            if (slots.Length < MaxEquipmentsAmount)
            {
                Debug.LogError($"{this}: Not enough weapon slots in ship view");
                return;
            }

            for (var i = 0; i < slots.Length; i++)
            {
                if (i >= MaxEquipmentsAmount)
                {
                    slots[i].gameObject.SetActive(false);
                    continue;
                }

                slots[i].gameObject.SetActive(true);
                Slots[i] = slots[i];
            }
        }

        public virtual async UniTask SetEquipmentAsync(int slotIndex, TEquipType equipType)
        {
            if (slotIndex >= MaxEquipmentsAmount)
            {
                Debug.LogError($"{this}: Cannot equip {equipType} to slot index {slotIndex} cause maximum amount is {MaxEquipmentsAmount}");
                return;
            }

            if (Equipments.TryGetValue(slotIndex, out var equipment))
                equipment?.Unequip();

            Equipments[slotIndex] = await EquipmentsFactory.CreateEquipment(equipType, Slots[slotIndex]);
            OnEquipmentChanged?.Invoke(slotIndex, equipType);
        }
    }
}
