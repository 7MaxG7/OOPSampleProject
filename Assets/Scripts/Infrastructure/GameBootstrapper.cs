using UnityEngine;
using Zenject;

namespace Infrastructure
{
    public sealed class GameBootstrapper : MonoBehaviour
    {
        private Game _game;

        [Inject]
        private void InjectDependencies(Game game)
        {
            _game = game;
        }

        private void Awake()
        {
            _game.Init();
            DontDestroyOnLoad(this);
        }

        private void Update() 
            => _game.Updater?.OnUpdate(Time.deltaTime);

        private void OnDestroy() 
            => _game.Cleaner?.CleanUp();
    }
}