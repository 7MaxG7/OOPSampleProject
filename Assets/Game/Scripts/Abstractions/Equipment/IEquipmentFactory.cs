using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Equipment
{
    public interface IEquipmentFactory<TEquipment, in TEquipType>
    {
        UniTask<TEquipment> CreateEquipment(TEquipType type, Transform parent);
    }
}
