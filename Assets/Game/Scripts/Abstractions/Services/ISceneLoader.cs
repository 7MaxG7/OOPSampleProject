using System.Threading;
using Cysharp.Threading.Tasks;

namespace Services
{
    public interface ISceneLoader
    {
        UniTask LoadSceneAsync(string sceneName, CancellationTokenSource cts);
        string GetCurrentSceneName();
    }
}