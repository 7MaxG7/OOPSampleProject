using System;
using System.Collections.Generic;
using Ships;
using UnityEngine;

namespace Equipment
{

    public abstract class BaseEquipmentBattery<TEquipment, TEquipType> : IEquipmentBattery<TEquipment, TEquipType>
        where TEquipment : IEquipment where TEquipType : Enum
    {
        public event Action<IShip, int, TEquipment> OnEquipmentChanged;

        public int MaxEquipmentsAmount { get; }
        public IEquipmentFactory<TEquipment, TEquipType> EquipmentsFactory { get; }
        public Dictionary<int, TEquipment> Equipments { get; } = new();
        protected IShip Owner;

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
        }
        
        public void Init(IShip owner)
        {
            Owner = owner;
        }

        public virtual void SetEquipment(int slotIndex, TEquipType equipType)
        {
            if (slotIndex >= MaxEquipmentsAmount)
            {
                Debug.LogError($"{this}: Cannot equip {equipType} to slot index {slotIndex} cause maximum amount is {MaxEquipmentsAmount}");
                return;
            }

            if (Equipments.TryGetValue(slotIndex, out var equipment))
                equipment?.Unequip();

            Equipments[slotIndex] = EquipmentsFactory.CreateEquipment(equipType);
            OnEquipmentChanged?.Invoke(Owner, slotIndex, Equipments[slotIndex]);
        }
    }
}