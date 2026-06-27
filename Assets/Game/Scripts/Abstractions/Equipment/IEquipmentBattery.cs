using System;
using System.Collections.Generic;
using Ships;

namespace Equipment
{
    public interface IEquipmentBattery<TEquipment, in TEquipType>
    {
        event Action<IShip, int, TEquipment> OnEquipmentChanged;

        int MaxEquipmentsAmount { get; }
        Dictionary<int, TEquipment> Equipments { get; }
        IEquipmentFactory<TEquipment, TEquipType> EquipmentsFactory { get; }

        void Init(IShip ship);
        void SetEquipment(int slotIndex, TEquipType equipType);
    }
}