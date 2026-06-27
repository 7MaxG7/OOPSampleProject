using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Ships;
using UI.Data;
using UnityEngine;
using Utils;

namespace UI.Ship
{
    public class EquipmentSelectView : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private RectTransform _equipmentsContent;
        [SerializeField] private OpponentAnchor[] _opponentAnchors;
        [SerializeField] private CanvasGroup _canvasGroup;
        
        public Transform EquipmentsContent => _equipmentsContent;
        
        private float _fadeAnimDuration;

        public void Init(float fadeAnimDuration)
        {
            _fadeAnimDuration = fadeAnimDuration;
            
            _canvasGroup.alpha = 0f;
            gameObject.SetActive(false);
        }

        public void Locate(OpponentId opponentId, Vector3 position)
        {
            var anchors = _opponentAnchors.FirstOrDefault(data => data.OpponentId == opponentId);
            if (anchors != null)
            {
                _rectTransform.anchorMin = anchors.Min;
                _rectTransform.anchorMax = anchors.Max;
                _rectTransform.pivot = anchors.Pivot;
            }
            else
                Debug.LogWarning($"Cannot find equipment anchor for {opponentId}");

            _rectTransform.position = position;
        }

        public async UniTask SetVisibleAsync(bool isVisible, CancellationToken token, float durationRate = 1f)
        {
            if (isVisible != IsVisible())
                _canvasGroup.blocksRaycasts = isVisible;
            await _canvasGroup.SetCanvasGroupVisibilityAsync(isVisible, _fadeAnimDuration * durationRate, token);
        }

        public bool IsVisible()
            => gameObject.activeSelf;
    }
}
