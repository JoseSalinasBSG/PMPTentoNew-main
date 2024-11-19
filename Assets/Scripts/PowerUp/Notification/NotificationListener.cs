using System;
using System.Collections;
using System.Collections.Generic;
using Notification;
using UnityEngine;
using UnityEngine.Events;

public class NotificationListener : MonoBehaviour
{
    [SerializeField] private ScriptableObjectNotification _objectNotification;
    [SerializeField] private UnityEvent _onInitresponse;
    [SerializeField] private UnityEvent _onFinishResponse;
    private void OnEnable()
    {
        _objectNotification.RegisterListener(this);
    }

    private void OnDisable()
    {
        _objectNotification.UnregisterListener(this);
    }

    public void OnEventRaise(float time)
    {
        StopAllCoroutines();
        StartCoroutine(ActiveNotification(time));
    }

    IEnumerator ActiveNotification(float time)
    {
        _onInitresponse?.Invoke();
        var currentTime = 0f;
        while (currentTime <= time)
        {
            currentTime += Time.deltaTime;
            yield return null;
        }
        gameObject.SetActive(false);
        _onFinishResponse?.Invoke();
        
    }
}
