using System.Collections.Generic;
using Notification;
using UnityEngine;

namespace ScriptableCreator
{
    [CreateAssetMenu(fileName = "IntNotification", menuName = "Int notification")]
    public class ScriptableObjectNotificationInt : ScriptableObject
    {
        public List<NotificationListenerInt> _listeners = new List<NotificationListenerInt>();

        public void Raise(int message)
        {
            for (int i = _listeners.Count - 1; i >= 0; i--)
            {
                _listeners[i].OnEventRaise(message);

            }
        }

        public void RegisterListener(NotificationListenerInt notificationListener)
        {
            _listeners.Add(notificationListener);
        }

        public void UnregisterListener(NotificationListenerInt notificationListener)
        {
            _listeners.Remove(notificationListener);
        }
    }
}