using System.Collections.Generic;
using UnityEngine;

namespace Equipment
{
    public interface IAbstractEquipments<TEquipment, TEquipType>
    {
        int MaxEquipmentsAmount { get; }
        Dictionary<int,TEquipment> Equipments { get; }
        IEquipmentFactory<TEquipment, TEquipType> EquipmentsFactory { get; }
        Dictionary<int,Transform> Slots { get; }
    }
}