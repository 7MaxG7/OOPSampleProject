using System.Collections.Generic;
using Equipment;
using UnityEngine;
using Zenject;

namespace Ships
{
    public sealed class ShipUpgrader : IShipUpgrader
    {
        private readonly Dictionary<BuffParamType, IUpgradeHandler> _upgradeHandlers;

        [Inject]
        public ShipUpgrader()
        {
            var healthBuffHandler = new HealthUpgradeHandler();
            _upgradeHandlers = new()
            {
                [BuffParamType.Shield] = healthBuffHandler,
                [BuffParamType.Hp] = healthBuffHandler,
                [BuffParamType.ShieldRecovery] = healthBuffHandler,
                [BuffParamType.ShootCooldown] = new WeaponCooldownUpgradeHandler(),
            };
        }
        
        public void Upgrade(IShip ship, IModule module)
        {
            if (!_upgradeHandlers.TryGetValue(module.BuffParamType, out var upgradeHandler))
            {
                Debug.LogError($"{this}: Upgrade handler for {module.BuffParamType} not found");
                return;
            }
            
            upgradeHandler.Upgrade(ship, module);
        }

        public void Downgrade(IShip ship, IModule module)
        {
            if (!_upgradeHandlers.TryGetValue(module.BuffParamType, out var upgradeHandler))
            {
                Debug.LogError($"{this}: Upgrade handler for {module.BuffParamType} not found");
                return;
            }
            
            upgradeHandler.Downgrade(ship, module);
        }
    }
}