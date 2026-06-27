using Equipment;
using Infrastructure;
using UnityEngine;

namespace Ui
{
    public interface IShipSetupUIService : ISceneCleanable
    {
        void Init();
        Sprite GetWeaponIcon(WeaponType weaponType);
        Sprite GetModuleIcon(ModuleType moduleType);
    }
}