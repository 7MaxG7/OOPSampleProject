using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Equipment
{
    public interface IEquipmentBattery<TEquipment, TEquipType>
    {
        event Action<int, TEquipType> OnEquipmentChanged;

        int MaxEquipmentsAmount { get; }
        Dictionary<int, TEquipment> Equipments { get; }
        IEquipmentFactory<TEquipment, TEquipType> EquipmentsFactory { get; }
        Dictionary<int, Transform> Slots { get; }

        void SetSlots(Transform[] moduleSlots);
        UniTask SetEquipmentAsync(int slotIndex, TEquipType equipType);
    }
}