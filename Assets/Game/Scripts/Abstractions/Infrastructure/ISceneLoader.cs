using System.Threading;
using Cysharp.Threading.Tasks;

namespace Infrastructure
{
    public interface ISceneLoader
    {
        UniTask LoadSceneAsync(string sceneName, CancellationTokenSource cts);
        string GetCurrentSceneName();
    }
}