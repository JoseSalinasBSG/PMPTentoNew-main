using System.Collections.Generic;
using UnityEngine;

namespace ScriptableCreator
{
    [CreateAssetMenu(fileName = "TextNotification", menuName = "Text notification")]
    public class ScriptableObjectNotificationText : ScriptableObject
    {
        public List<NotificationListenerText> _listeners = new List<NotificationListenerText>();
        public void Raise(string message)
        {
            for (int i = _listeners.Count -1 ; i >= 0; i--)
            {
                _listeners[i].OnEventRaise(message);

            }
        }

        public void RegisterListener(NotificationListenerText notificationListener)
        {
            _listeners.Add(notificationListener);
        }

        public void UnregisterListener(NotificationListenerText notificationListener)
        {
            _listeners.Remove(notificationListener);
        }
    }
}
