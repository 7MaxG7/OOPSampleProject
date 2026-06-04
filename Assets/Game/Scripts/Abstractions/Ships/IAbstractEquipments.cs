using System.Collections.Generic;
using Services;
using UnityEngine;

namespace Ships
{
    public interface IAbstractEquipments<TEquipment, TEquipType>
    {
        int MaxEquipmentsAmount { get; }
        Dictionary<int,TEquipment> Equipments { get; }
        IEquipmentFactory<TEquipment, TEquipType> EquipmentsFactory { get; }
        Dictionary<int,Transform> Slots { get; }
    }
}