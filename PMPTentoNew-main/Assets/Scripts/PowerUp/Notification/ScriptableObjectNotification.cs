using System.Collections.Generic;
using UnityEngine;

namespace Notification
{
    [CreateAssetMenu(fileName = "Notification", menuName = "Notification")]
    public class ScriptableObjectNotification : ScriptableObject
    {
        public List<NotificationListener> _listeners = new List<NotificationListener>();
        public float notificationActivityTime;
        public void Raise()
        {
            for (int i = _listeners.Count -1 ; i >= 0; i--)
            {
                _listeners[i].OnEventRaise(notificationActivityTime);

            }
        }

        public void RegisterListener(NotificationListener notificationListener)
        {
            _listeners.Add(notificationListener);
        }

        public void UnregisterListener(NotificationListener notificationListener)
        {
            _listeners.Remove(notificationListener);
        }
    }
}
