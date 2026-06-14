using UnityEngine;

namespace Ships
{
    [CreateAssetMenu(menuName = "Configs/" + nameof(RulesConfig), fileName = nameof(RulesConfig))]
    public class RulesConfig : ScriptableObject
    {
        [SerializeField] private Opponent[] _opponents;

        public Opponent[] Opponents => _opponents;
    }
}