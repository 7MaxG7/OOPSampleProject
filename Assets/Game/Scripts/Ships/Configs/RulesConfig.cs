using System.Linq;
using Ships.Data;
using UnityEngine;

namespace Ships
{
    [CreateAssetMenu(menuName = "Configs/" + nameof(RulesConfig), fileName = nameof(RulesConfig))]
    public class RulesConfig : ScriptableObject
    {
        [SerializeField] private Opponent[] _opponents;

        public Opponent[] Opponents => _opponents;

        public SpawnPosition GetSceneLocation(OpponentId opponentId, string sceneName) 
            => _opponents.FirstOrDefault(data => data.OpponentId == opponentId)?
                .SpawnPositions.FirstOrDefault(data => data.SceneName == sceneName);
    }
}