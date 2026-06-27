namespace Equipment
{
    public interface IEquipmentFactory<TEquipment, in TEquipType>
    {
        TEquipment CreateEquipment(TEquipType type);
    }
}