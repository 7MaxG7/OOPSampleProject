using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Utils
{
    public static class UIExtensions
    {
        public static async UniTask SetCanvasGroupVisibilityAsync(this CanvasGroup canvasGroup, bool mustVisible,
            float animationDuration, CancellationToken token)
        {
            canvasGroup.DOKill();
            if (mustVisible && !canvasGroup.IsVisible())
            {
                canvasGroup.blocksRaycasts = true;
                if (!canvasGroup.gameObject.activeSelf)
                {
                    canvasGroup.gameObject.SetActive(true);
                    canvasGroup.alpha = 0;
                }

                await canvasGroup.DOFade(1, animationDuration)
                    .SetUpdate(true)
                    .WithCancellation(token)
                    .SuppressCancellationThrow();
            }
            else if (!mustVisible && canvasGroup.gameObject.activeSelf)
            {
                canvasGroup.blocksRaycasts = false;
                await canvasGroup.DOFade(0, animationDuration)
                    .SetUpdate(true)
                    .WithCancellation(token)
                    .SuppressCancellationThrow();
                canvasGroup.gameObject.SetActive(false);
            }
        }

        public static bool IsVisible(this CanvasGroup canvasGroup)
            => canvasGroup.gameObject.activeSelf && canvasGroup.alpha.IsAlmostOne();
    }
}