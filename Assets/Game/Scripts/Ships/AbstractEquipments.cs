using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Services;
using UnityEngine;

namespace Ships
{

    public abstract class AbstractEquipments<TEquipment, TEquipType> : IAbstractEquipments<TEquipment, TEquipType>
        where TEquipment : IEquipment where TEquipType : Enum
    {
        public int MaxEquipmentsAmount { get; }
        public IEquipmentFactory<TEquipment, TEquipType> EquipmentsFactory { get; }
        public Dictionary<int, TEquipment> Equipments { get; } = new();
        public Dictionary<int, Transform> Slots { get; } = new();


        protected AbstractEquipments(int amount, IEquipmentFactory<TEquipment, TEquipType> equipmentFactory)
        {
            MaxEquipmentsAmount = amount;
            EquipmentsFactory = equipmentFactory;
        }

        protected AbstractEquipments(IAbstractEquipments<TEquipment, TEquipType> baseEquipments)
        {
            MaxEquipmentsAmount = baseEquipments.MaxEquipmentsAmount;
            Equipments = baseEquipments.Equipments;
            EquipmentsFactory = baseEquipments.EquipmentsFactory;
            Slots = baseEquipments.Slots;
        }
        
        public void SetSlots(Transform[] slots)
        {
            if (slots.Length < MaxEquipmentsAmount)
            {
                Debug.LogError($"{this}: Not enought weapon slots in ship view");
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

        public virtual async UniTask SetEquipmentAsync(int index, TEquipType equipType)
        {
            if (index >= MaxEquipmentsAmount)
                return;

            if (!Equipments.TryGetValue(index, out var equipment))
                Equipments.Add(index, default);
            else
                equipment?.Unequip();

            Equipments[index] = await EquipmentsFactory.CreateEquipment(equipType, Slots[index]);
        }
  
        public void SetEquipmentSync(int index, TEquipType equipType)
        {
#pragma warning disable CS4014
            SetEquipmentAsync(index, equipType);
#pragma warning restore CS4014
        }
    }
}
