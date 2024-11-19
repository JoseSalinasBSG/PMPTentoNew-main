using System.Collections;
using System.Collections.Generic;
using ScriptableCreator;
using UnityEngine;
using UnityEngine.Events;

public class NotificationListenerText : MonoBehaviour
{
    [SerializeField] private ScriptableObjectNotificationText _objectNotification;
    [SerializeField] private UnityEvent<string>  _onResponse;

    private void OnEnable()
    {
        _objectNotification.RegisterListener(this);
    }

    private void OnDisable()
    {
        _objectNotification.UnregisterListener(this);
    }
    public void OnEventRaise(string time)
    {
        _onResponse?.Invoke(time);
    }
}
