using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Ui
{
    public sealed class CurtainView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;

        private float _animationDuration;


        public void Init(float animationDuration)
        {
            _animationDuration = animationDuration;
            DontDestroyOnLoad(this);
        }

        public async UniTask SetCurtainVisibleAsync(bool isVisible, CancellationToken token)
            => await _canvasGroup.SetCanvasGroupVisibilityAsync(isVisible, _animationDuration, token);

        public void SetCurtainVisibleInstantly(bool isVisible)
        {
            _canvasGroup.DOKill();
            _canvasGroup.alpha = isVisible ? 1f : 0f;
            gameObject.SetActive(isVisible);
        }
    }
}