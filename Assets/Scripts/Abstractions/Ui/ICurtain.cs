using System.Threading;
using Cysharp.Threading.Tasks;

namespace Abstractions.Ui
{
    public interface ICurtain
    {
        UniTask InitAsync();
        UniTask SetCurtainVisibleAsync(bool isVisible, CancellationToken token);
        void ShowCurtainInstantly();
    }
}
