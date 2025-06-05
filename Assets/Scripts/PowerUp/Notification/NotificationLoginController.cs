using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

///<summary>
/// Controlador de notificaciones para el inicio de sesión.
/// Maneja la visualización y el flujo de notificaciones de error y éxito,
/// permitiendo mostrar mensajes personalizados por un tiempo definido y ejecutando eventos al finalizar.
///</summary>

namespace Notification
{
    public class NotificationLoginController : MonoBehaviour
    {
        [SerializeField] private Transform _loginErrorNotification;
        [SerializeField] private Transform _loginSuccessNotification;
        [SerializeField] private TextMeshProUGUI _messageErrorNotification;
        [SerializeField] private float _NotificationDuration;
        [SerializeField] private UnityEvent _onFinishNotificationError;
        [SerializeField] private UnityEvent _onFinishNotificationSuccess;

        public void StartErrorNotification(string errorMessage)
        {
            _messageErrorNotification.text = errorMessage;
            StopAllCoroutines();
            StartCoroutine(IStartNotification(true));
        }

        public void StartSuccessNotification()
        {
            StopAllCoroutines();
            StartCoroutine(IStartNotification(false));
        }

        IEnumerator IStartNotification(bool isError)
        {
            var currentTime = 0f;
            Transform _target = null;
            if (isError)
            {
                _target = _loginErrorNotification;
            }
            else
            {
                _target = _loginSuccessNotification;
            }
            _target.gameObject.SetActive(true);
            while (currentTime <= _NotificationDuration)
            {
                currentTime += Time.deltaTime;
                yield return null;
            }
            _target.gameObject.SetActive(false);
            if (isError)
            {
                _onFinishNotificationError?.Invoke();

            }
            else
            {
                _onFinishNotificationSuccess?.Invoke();

            }
        }

    }
}
