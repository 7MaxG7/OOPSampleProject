using UnityEngine;
using UnityEngine.UI;

namespace Ui.ShipSetup
{
    public class SlotUiView : MonoBehaviour
    {
        [SerializeField] private Button _selectButton;
        [SerializeField] private Image _icon;
        
        public Button SelectButton => _selectButton;

        public void SetIcon(Sprite icon)
        {
            _icon.gameObject.SetActive(icon != null);
            _icon.sprite = icon;
        }
    }
}