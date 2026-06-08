using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Equipment
{
    public interface IAbstractEquipments<TEquipment, in TEquipType>
    {
        int MaxEquipmentsAmount { get; }
        Dictionary<int,TEquipment> Equipments { get; }
        IEquipmentFactory<TEquipment, TEquipType> EquipmentsFactory { get; }
        Dictionary<int,Transform> Slots { get; }
        
        void SetSlots(Transform[] moduleSlots);
        void SetEquipment(int index, TEquipType equipType);
        UniTask SetEquipmentAsync(int slot, TEquipType equipType);
    }
}