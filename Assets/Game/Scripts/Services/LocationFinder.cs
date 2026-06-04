using System.Linq;
using Configs;
using Configs.Data;
using Enums;
using Ships.Views;
using UnityEngine;
using Zenject;

namespace Services
{
    public sealed class LocationFinder : ILocationFinder
    {
        private readonly ISceneLoader _sceneLoader;
        private readonly RulesConfig _rulesConfig;


        [Inject]
        public LocationFinder(ISceneLoader sceneLoader, RulesConfig rulesConfig)
        {
            _sceneLoader = sceneLoader;
            _rulesConfig = rulesConfig;
        }
        
        public Vector3? GetOpponentLocation(OpponentId opponentId, out Quaternion rotation)
        {
            var sceneName = _sceneLoader.GetCurrentSceneName();
            rotation = default;
            
            var locationRule = _rulesConfig.GetSceneLocation(opponentId, sceneName)
                               ?? FindLocationAtScene(opponentId, sceneName);

            if (locationRule != null)
                rotation = locationRule.Rotation;
            return locationRule?.Position;
        }

        private SpawnPosition FindLocationAtScene(OpponentId opponentId, string sceneName)
        {
            Debug.LogError($"{this}: No spawn position in rules config for opponent {opponentId.ToString()} in scene {sceneName}");
            
            var shipSpawnerMarker = Object.FindObjectsOfType<ShipSpawnerMarker>()
                .FirstOrDefault(data => data.OpponentId == opponentId);
            if (shipSpawnerMarker == null)
                return null;

            var sceneRule = new SpawnPosition(sceneName);
            sceneRule.UpdatePosition(shipSpawnerMarker.transform);
            return sceneRule;
        }
    }
}