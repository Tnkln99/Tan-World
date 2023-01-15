using Helpers.Utilities;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Helpers.HelperComponents
{
    public class BaseButton : MonoBehaviour
    {
        public UnityEvent ClickAction;
        public bool PlayButtonSound;
        private Button _button;
        private bool _isClickable = true;
        
        private void Start()
        {
            if (_button != null)
            {
                Destroy(_button);
            }
            _button = gameObject.AddComponent<Button>();
            _button.transition = Selectable.Transition.None;
            _button.onClick.RemoveAllListeners();
            _button.onClick.AddListener(OnClick);
            _button.interactable = true;
        }
        

        public void SetClickableStatus(bool isClickable)
        {
            _isClickable = isClickable;
        }

        public bool IsClickable()
        {
            return _isClickable && this.IsInputActive();
        }

        private void OnClick()
        {
            if (!IsClickable())
            {
                return;
            }
            ClickAction.Invoke();
            //todo: add sound
        }
    }
}