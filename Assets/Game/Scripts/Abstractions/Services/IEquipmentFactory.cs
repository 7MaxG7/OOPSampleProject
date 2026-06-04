using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Services
{
    public interface IEquipmentFactory<TEquipment, in TEquipType>
    {
        UniTask<TEquipment> CreateEquipment(TEquipType type, Transform parent);
    }
}
