using System.Collections.Generic;
using System.Linq;
using Battle;
using Cysharp.Threading.Tasks;
using Equipment;
using Infrastructure;
using Sounds;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace Ships
{
    public sealed class ShipsViewInitializer : IShipsViewInitializer
    {
        private readonly IShipViewFactory _shipViewFactory;
        private readonly IShipConfigurator _shipConfigurator;
        private readonly ISoundService _soundService;
        private readonly IDamageableIdentifier _damageableIdentifier;
        private readonly IWeaponShotService _weaponShotService;
        private readonly IEquipmentIdentifier _equipmentIdentifier;

        private readonly Dictionary<OpponentId, ShipView> _shipViews = new();

        [Inject]
        public ShipsViewInitializer(IShipViewFactory shipViewFactory, IShipConfigurator shipConfigurator,
            ISoundService soundService, IDamageableIdentifier damageableIdentifier, IWeaponShotService weaponShotService,
            IEquipmentIdentifier equipmentIdentifier, ICleaner cleaner)
        {
            _shipViewFactory = shipViewFactory;
            _shipConfigurator = shipConfigurator;
            _soundService = soundService;
            _damageableIdentifier = damageableIdentifier;
            _weaponShotService = weaponShotService;
            _equipmentIdentifier = equipmentIdentifier;

            cleaner.AddCleanable(this);
        }

        public void CleanUp()
        {
            foreach (var (opponentId, ship) in _shipConfigurator.Ships)
            {
                ship.WeaponBattery.OnShoot -= _soundService.PlayShoot;
                ship.OnDied -= DestroyShipView;
                ship.WeaponBattery.OnEquipmentChanged -= CreateWeaponView;
                ship.ModuleBattery.OnEquipmentChanged -= CreateModuleView;
                foreach (var weapon in ship.WeaponBattery.Equipments.Values)
                {
                    weapon.OnUnequip -= UnequipWeapon;
                    _weaponShotService.UnregisterWeapon(weapon);
                }
                foreach (var module in ship.ModuleBattery.Equipments.Values)
                    module.OnUnequip -= UnequipModule;
                if (_shipViews.TryGetValue(opponentId, out var shipView))
                    ship.Health.OnShieldChanged -= shipView.Shield.UpdatePower;
            }

            _shipViews.Clear();
        }

        public async UniTask CreateShipsViewsAsync()
        {
            var spawnLocations = Object.FindObjectsOfType<ShipSpawnerMarker>()
                .ToDictionary(data => data.OpponentId, data => (data.transform.position, data.transform.rotation));

            foreach (var (opponentId, ship) in _shipConfigurator.Ships)
            {
                var location = spawnLocations.GetValueOrDefault(opponentId);
                var shipView = await _shipViewFactory.CreateShipViewAsync(ship, location.position, location.rotation);

                foreach (var (slotIndex, weapon) in ship.WeaponBattery.Equipments)
                    await CreateWeaponViewAsync(shipView, slotIndex, weapon);
                foreach (var (slotIndex, module) in ship.ModuleBattery.Equipments)
                    await shipView.CreateModuleViewAsync(slotIndex, module.ModuleType);

                ship.WeaponBattery.OnShoot += _soundService.PlayShoot;
                ship.OnDied += DestroyShipView;
                ship.WeaponBattery.OnEquipmentChanged += CreateWeaponView;
                ship.ModuleBattery.OnEquipmentChanged += CreateModuleView;
                foreach (var weapon in ship.WeaponBattery.Equipments.Values)
                    weapon.OnUnequip += UnequipWeapon;
                foreach (var module in ship.ModuleBattery.Equipments.Values)
                    module.OnUnequip += UnequipModule;
                ship.Health.OnShieldChanged += shipView.Shield.UpdatePower;

                shipView.Shield.UpdatePower(ship.Health.CurrentShield, ship.Health.MaxShield);
                _damageableIdentifier.AddShip(ship, shipView);
                _shipViews.Add(opponentId, shipView);
            }
        }

        private void CreateWeaponView(IShip ship, int slotIndex, IWeapon weapon)
        {
            weapon.OnUnequip += UnequipWeapon;
            if (TryGetShipView(ship, out var view, out _))
                CreateWeaponViewAsync(view, slotIndex, weapon).Forget();
        }

        private void CreateModuleView(IShip ship, int slotIndex, IModule module)
        {
            module.OnUnequip += UnequipModule;
            if (TryGetShipView(ship, out var shipView, out _))
                shipView.CreateModuleViewAsync(slotIndex, module.ModuleType).Forget();
        }

        private async UniTask CreateWeaponViewAsync(ShipView shipView, int slotIndex, IWeapon weapon)
        {
            var weaponView = await shipView.CreateWeaponViewAsync(slotIndex, weapon.WeaponType);
            if (weaponView != null)
                _weaponShotService.RegisterWeapon(weapon, weaponView);
        }

        private void UnequipWeapon(IWeapon weapon)
        {
            weapon.OnUnequip -= UnequipWeapon;
            _weaponShotService.UnregisterWeapon(weapon);

            if (!_equipmentIdentifier.TryGetWeaponOwner(weapon, out var ship) || !TryGetShipView(ship, out var shipView, out _))
                return;

            foreach (var (slotIndex, equippedWeapon) in ship.WeaponBattery.Equipments)
            {
                if (equippedWeapon != weapon)
                    continue;

                shipView.UnequipWeaponView(slotIndex);
                return;
            }
        }

        private void UnequipModule(IModule module)
        {
            module.OnUnequip -= UnequipModule;
            if (!_equipmentIdentifier.TryGetModuleOwner(module, out var ship) || !TryGetShipView(ship, out var shipView, out _))
                return;

            foreach (var (slotIndex, equippedModule) in ship.ModuleBattery.Equipments)
            {
                if (equippedModule != module)
                    continue;

                shipView.UnequipModuleView(slotIndex);
                return;
            }
        }

        private void DestroyShipView(IShip ship)
        {
            if (!TryGetShipView(ship, out var shipView, out var opponentId))
                return;

            ship.WeaponBattery.OnShoot -= _soundService.PlayShoot;
            ship.OnDied -= DestroyShipView;
            ship.WeaponBattery.OnEquipmentChanged -= CreateWeaponView;
            ship.ModuleBattery.OnEquipmentChanged -= CreateModuleView;
            foreach (var weapon in ship.WeaponBattery.Equipments.Values)
            {
                weapon.OnUnequip -= UnequipWeapon;
                _weaponShotService.UnregisterWeapon(weapon);
            }
            foreach (var module in ship.ModuleBattery.Equipments.Values)
                module.OnUnequip -= UnequipModule;
            ship.Health.OnShieldChanged -= shipView.Shield.UpdatePower;

            Object.Destroy(shipView.gameObject);
            _shipViews.Remove(opponentId);
        }

        private bool TryGetShipView(IShip ship, out ShipView view, out OpponentId resultOpponentId)
        {
            foreach (var (opponentId, opponentShip) in _shipConfigurator.Ships)
            {
                if (opponentShip != ship)
                    continue;

                resultOpponentId = opponentId;
                return _shipViews.TryGetValue(resultOpponentId, out view);
            }

            Debug.LogError($"{this}: Cannot get view for ship {ship.Name}");
            view = null;
            resultOpponentId = OpponentId.None;
            return false;
        }
    }
}