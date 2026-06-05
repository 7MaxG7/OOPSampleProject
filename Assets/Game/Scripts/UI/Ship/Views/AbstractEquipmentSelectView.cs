using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Infrastructure.ControllersHolder;
using Ships.Data;
using Ui;
using UI.Data;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI.Ship.Views
{
    public abstract class AbstractEquipmentSelectView<TType> : MonoBehaviour, ICleanable where TType : Enum
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private RectTransform _equipmentsContent;
        [SerializeField] private OpponentAnchor[] _opponentAnchors;
        [SerializeField] private CanvasGroup _canvasGroup;
        
        protected IUiFactory UiFactory;
        protected Transform EquipmentsContent => _equipmentsContent;
        
        private readonly List<SlotUiView> _equipmentsSlots = new();
        private float _fadeAnimDuration;

        protected abstract UniTask<SlotUiView> CreateSelectUiSlot(TType equipmentType);

        public void Init(IUiFactory uiFactory, float fadeAnimDuration)
        {
            UiFactory = uiFactory;
            _fadeAnimDuration = fadeAnimDuration;
            
            _canvasGroup.alpha = 0f;
            gameObject.SetActive(false);
        }

        public void CleanUp()
        {
            foreach (var slot in _equipmentsSlots)
            {
                slot.SelectButton.onClick.RemoveAllListeners();
                if (slot != null && slot.gameObject != null)
                    Destroy(slot.gameObject);
            }
            _equipmentsSlots.Clear();
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
            => await _canvasGroup.SetCanvasGroupVisibilityAsync(isVisible, _fadeAnimDuration * durationRate, token);

        public async UniTask<Button> AddEquipmentSelectSlot(TType equipmentType)
        {
            var selectUiSlot = await CreateSelectUiSlot(equipmentType);
            _equipmentsSlots.Add(selectUiSlot);
            return selectUiSlot.SelectButton;
        }

        public bool IsVisible()
            => gameObject.activeSelf;
    }
}
