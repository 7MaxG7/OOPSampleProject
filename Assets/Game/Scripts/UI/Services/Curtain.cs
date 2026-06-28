using System.Threading;
using Cysharp.Threading.Tasks;
using Ui;
using Zenject;

namespace UI
{
    public sealed class Curtain : ICurtain
    {
        private readonly IUIFactory _uiFactory;

        private CurtainUIView _curtainView;
        
        [Inject]
        public Curtain(IUIFactory uiFactory)
        {
            _uiFactory = uiFactory;
        }

        public void Init()
        {
            _curtainView = _uiFactory.CreateCurtain();
        }

        public async UniTask SetCurtainVisibleAsync(bool isVisible, CancellationToken token)
            => await _curtainView.SetCurtainVisibleAsync(isVisible, token);

        public void ShowCurtainInstantly()
            => _curtainView.SetCurtainVisibleInstantly(true);
    }
}
