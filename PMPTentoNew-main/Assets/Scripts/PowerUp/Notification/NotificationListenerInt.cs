using ScriptableCreator;
using UnityEngine;
using UnityEngine.Events;

namespace Notification
{
    public class NotificationListenerInt : MonoBehaviour
    {
        [SerializeField] private ScriptableObjectNotificationInt _objectNotification;
        [SerializeField] private UnityEvent<string> _onResponse;

        private void OnEnable()
        {
            _objectNotification.RegisterListener(this);
        }

        private void OnDisable()
        {
            _objectNotification.UnregisterListener(this);
        }

        public void OnEventRaise(int time)
        {
            _onResponse?.Invoke(time.ToString());
        }
    }
}
