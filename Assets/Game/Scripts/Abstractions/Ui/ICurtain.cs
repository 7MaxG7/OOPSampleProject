using System.Threading;
using Cysharp.Threading.Tasks;

namespace Ui
{
    public interface ICurtain
    {
        void Init();
        UniTask SetCurtainVisibleAsync(bool isVisible, CancellationToken token);
        void ShowCurtainInstantly();
    }
}
