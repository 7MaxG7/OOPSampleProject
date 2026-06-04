using System.Threading;
using Abstractions.Services;
using Abstractions.Ui;
using Cysharp.Threading.Tasks;
using Zenject;

namespace Ui
{
    public sealed class Curtain : ICurtain
    {
        private readonly IUiFactory _uiFactory;

        private CurtainView _curtainView;
        
        [Inject]
        public Curtain(IUiFactory uiFactory)
        {
            _uiFactory = uiFactory;
        }

        public async UniTask InitAsync()
        {
            _curtainView = await _uiFactory.CreateCurtainAsync();
        }

        public async UniTask SetCurtainVisibleAsync(bool isVisible, CancellationToken token)
            => await _curtainView.SetCurtainVisibleAsync(isVisible, token);

        public void ShowCurtainInstantly()
            => _curtainView.SetCurtainVisibleInstantly(true);
    }
}
