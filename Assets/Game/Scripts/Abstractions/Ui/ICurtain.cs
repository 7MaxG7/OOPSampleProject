using System.Threading;
using Cysharp.Threading.Tasks;

namespace Ui
{
    public interface ICurtain
    {
        UniTask InitAsync();
        UniTask SetCurtainVisibleAsync(bool isVisible, CancellationToken token);
        void ShowCurtainInstantly();
    }
}
