using System.Threading;
using Cysharp.Threading.Tasks;
using Infrastructure.ControllersHolder;
using UnityEngine.SceneManagement;
using Zenject;

namespace Infrastructure.Wrappers
{
    internal sealed class SceneLoader : ISceneLoader
    {
        private readonly ICleaner _cleaner;

        [Inject]
        public SceneLoader(ICleaner cleaner)
        {
            _cleaner = cleaner;
        }

        public async UniTask LoadSceneAsync(string sceneName, CancellationTokenSource cts)
        {
            if (GetCurrentSceneName() == sceneName)
                return;

            _cleaner.SceneCleanUp();
            await SceneManager.LoadSceneAsync(sceneName).WithCancellation(cts.Token);
        }

        public string GetCurrentSceneName() 
            => SceneManager.GetActiveScene().name;
    }
}