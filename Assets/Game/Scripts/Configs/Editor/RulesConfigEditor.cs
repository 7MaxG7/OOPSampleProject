using System.Linq;
using Configs.Data;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Configs.Editor
{
    [CustomEditor(typeof(RulesConfig))]
    public class RulesConfigEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var rulesConfig = (RulesConfig)target;

            var sceneName = SceneManager.GetActiveScene().name;
            if (GUILayout.Button("Update spawn locations"))
            {
                var spawnerMarkers = FindObjectsOfType<Ships.Views.ShipSpawnerMarker>();
                foreach (var marker in spawnerMarkers)
                {
                    var opponentRule = rulesConfig.Opponents.FirstOrDefault(data => data.OpponentId == marker.OpponentId);
                    if (opponentRule == null)
                        continue;

                    var sceneRule = opponentRule.SpawnPositions.FirstOrDefault(data => data.SceneName == sceneName);
                    if (sceneRule == null)
                    {
                        sceneRule = new SpawnPosition(sceneName);
                        opponentRule.SpawnPositions.Add(sceneRule);
                    }

                    sceneRule.UpdatePosition(marker.transform);
                }
            }
            
            EditorUtility.SetDirty(target);
        }
    }
}