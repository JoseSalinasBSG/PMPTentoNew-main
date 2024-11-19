using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_IOS
using Unity.Notifications.iOS;
#endif
using UnityEngine;

public class BackgroundNotificationIos
{
    public string Identifier;
    public string Title;
    public string Body;
    public string Subtitle;
    public int FireTimeInHours;
    public int FireTimeInMinutes;
    public int FireTimeInSeconds;

}
public class BackgroundNotificationControllerIos : MonoBehaviour
{
    private void OnEnable()
    {
        GameEvents.SendNotificationIos += SendNotification;
    }

    private void OnDisable()
    {
        GameEvents.SendNotificationIos -= SendNotification;
    }
#if UNITY_IOS
    private IEnumerator RequestAuthorization()
    {
        var authorizationOption = AuthorizationOption.Alert | AuthorizationOption.Badge;
        using (var req = new AuthorizationRequest(authorizationOption, true))
        {
            while (!req.IsFinished)
            {
                yield return null;
            };

            string res = "\n RequestAuthorization:";
            res += "\n finished: " + req.IsFinished;
            res += "\n granted :  " + req.Granted;
            res += "\n error:  " + req.Error;
            res += "\n deviceToken:  " + req.DeviceToken;
            Debug.Log(res);
        }
    }

    private void CreateNotification(BackgroundNotificationIos backgroundNotificationIos)
    {
        var timeTrigger = new iOSNotificationTimeIntervalTrigger()
        {
            TimeInterval = new TimeSpan(
                backgroundNotificationIos.FireTimeInHours, 
                backgroundNotificationIos.FireTimeInMinutes, 
                backgroundNotificationIos.FireTimeInSeconds),
            Repeats = false
        };

        var notification = new iOSNotification()
        {
            Identifier = backgroundNotificationIos.Identifier,
            Title = backgroundNotificationIos.Title,
            Body = backgroundNotificationIos.Body,
            Subtitle = backgroundNotificationIos.Subtitle,
            ShowInForeground = true,
            ForegroundPresentationOption = (PresentationOption.Alert | PresentationOption.Sound),
            CategoryIdentifier = "category_a",
            ThreadIdentifier = "thread1",
            Trigger = timeTrigger,
        };

        iOSNotificationCenter.ScheduleNotification(notification);
        NotificationCallback();
    }

    private void NotificationCallback()
    {
        iOSNotificationCenter.OnRemoteNotificationReceived += remoteNotification =>
        {
            Debug.Log($"CALLBACK: {remoteNotification.Identifier}/{remoteNotification.Title}/{remoteNotification.Body}");
            NotificationPanel notificationPanel = new NotificationPanel { 
                Title = remoteNotification.Title,
                Message = remoteNotification.Body
            };
            GameEvents.AddNotificationPanel?.Invoke(notificationPanel);
        };
    }
#endif

    private void SendNotification(BackgroundNotificationIos backgroundNotificationIos)
    {
#if UNITY_IOS
        StartCoroutine(RequestAuthorization());
        CreateNotification(backgroundNotificationIos);
#endif
    }
}
