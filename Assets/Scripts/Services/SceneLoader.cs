using System.Threading;
using Abstractions.Infrastructure;
using Abstractions.Services;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using Zenject;


namespace Services
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