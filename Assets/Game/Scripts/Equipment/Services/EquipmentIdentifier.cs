using Ships;
using UnityEngine;
using Zenject;

namespace Equipment
{
    public sealed class EquipmentIdentifier : IEquipmentIdentifier
    {
        private readonly IShipConfigurator _shipConfigurator;

        [Inject]
        public EquipmentIdentifier(IShipConfigurator shipConfigurator)
        {
            _shipConfigurator = shipConfigurator;
        }
        
        public bool TryGetModuleOwner(IModule module, out IShip owner)
        {
            foreach (var ship in _shipConfigurator.Ships.Values)
            foreach (var shipModule in ship.ModuleBattery.Equipments.Values)
            {
                if (shipModule != module)
                    continue;

                owner = ship;
                return true;
            }

            Debug.LogError($"{this}: Cannot find owner of module");
            owner = null;
            return false;
        }

        public bool TryGetWeaponOwner(IWeapon weapon, out IShip owner)
        {
            foreach (var ship in _shipConfigurator.Ships.Values)
            foreach (var shipWeapon in ship.WeaponBattery.Equipments.Values)
            {
                if (shipWeapon != weapon)
                    continue;

                owner = ship;
                return true;
            }

            Debug.LogError($"{this}: Cannot find owner of weapon");
            owner = null;
            return false;

        }
    }
}