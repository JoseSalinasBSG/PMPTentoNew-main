using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
namespace UI
{
    public class Popup : MonoBehaviour
    {
        [SerializeField] private RectTransform _container;
        [SerializeField] private UnityEvent _onOutsidePopup;
        [SerializeField] private bool _detectTouchOutside = true;

        public void OnEnable()
        {
            EnableInteraction();
        }

        public void DeleteInteraction()
        {
            if (_detectTouchOutside)
            {
                Touch.onFingerDown -= TouchOnonFingerDown;
                EnhancedTouchSupport.Disable();
            }
        }

        public void EnableInteraction()
        {
            if (_detectTouchOutside)
            {
                EnhancedTouchSupport.Enable();
                Touch.onFingerDown += TouchOnonFingerDown;
            }
        }

        private void TouchOnonFingerDown(Finger obj)//metodo para que cuando se toque fuera del gameobject _container ejecute una accion
        {
            var pos = obj.currentTouch.screenPosition;
            if (_container.gameObject.activeSelf && !RectTransformUtility.RectangleContainsScreenPoint(_container, pos))
            {
                _onOutsidePopup?.Invoke();
            }
        }

        private void OnDisable()
        {
            DeleteInteraction();
        }
    }
}
