using UnityEngine;
using System;
using System.Collections;

#if UNITY_ANDROID
using Unity.Notifications.Android;
#endif

public class BackgroundNotification
{
    public string IdCanal;
    public string NameCanal;
    public string DescriptionCanal;
    public string TitleNotification;
    public string TextNotification;
    public DateTime ActivationTime;
}

public class BackgroundNotificationController : MonoBehaviour
{
    private bool callbackWasSubscribed = false;

    private void OnEnable()
    {
        GameEvents.SendNotification += SendNotification;
    }

    private void OnDisable()
    {
        GameEvents.SendNotification -= SendNotification;
    }

    private void Start()
    {
#if UNITY_ANDROID
        StartCoroutine(NotificationPermission());
#endif
        
    }


#if UNITY_ANDROID

    private void CreateNotification(BackgroundNotification backgroundNotification)
    {
        AndroidNotificationChannel androidNotificationChannel = new AndroidNotificationChannel
        {
            Id = backgroundNotification.IdCanal,
            Name = backgroundNotification.NameCanal,
            Description = backgroundNotification.DescriptionCanal,
            Importance = Importance.Default
        };

        AndroidNotificationCenter.RegisterNotificationChannel(androidNotificationChannel);

        AndroidNotification androidNotification = new AndroidNotification
        {
            Title = backgroundNotification.TitleNotification,
            Text = backgroundNotification.TextNotification,
            SmallIcon = "default",
            LargeIcon = "default",
            FireTime = backgroundNotification.ActivationTime,
            IntentData = $"Arbritrary Text - Title: {backgroundNotification.TitleNotification} - Data 200"
        };

        AndroidNotificationCenter.SendNotification(androidNotification, backgroundNotification.IdCanal);
        NotificationCallback();
    }

    private void NotificationCallback()
    {
        AndroidNotificationCenter.NotificationReceivedCallback receivedNotificationHandler = delegate (AndroidNotificationIntentData data)
        {
            Debug.Log($"CALLBACK: {data.Id}/{data.Notification.Text}/{data.Channel}");

            NotificationPanel notificationPanel = new NotificationPanel { 
                Title = data.Notification.Title,
                Message = data.Notification.Text
            };
            GameEvents.AddNotificationPanel?.Invoke(notificationPanel);
        };

        var notificationIntentData = AndroidNotificationCenter.GetLastNotificationIntent();

        if (!callbackWasSubscribed)
        {
            AndroidNotificationCenter.OnNotificationReceived += receivedNotificationHandler;
            callbackWasSubscribed = true;
        }
    }

    public void GetStoredDataNotification() 
    {
        var notificationIntentData = AndroidNotificationCenter.GetLastNotificationIntent();
        if (notificationIntentData != null)
        {
            var id = notificationIntentData.Id;
            var channel = notificationIntentData.Channel;
            var notification = notificationIntentData.Notification;
            var arbritraryData = notificationIntentData.Notification.IntentData;
            Debug.Log($"DATA RECIEVED: {id}/{channel}/{notification}/{arbritraryData}");
        }
    }

   

#endif

    private void SendNotification(BackgroundNotification backgroundNotification)
    { 
        
#if UNITY_ANDROID
        CreateNotification(backgroundNotification);
#endif
    }
#if UNITY_ANDROID
    private IEnumerator NotificationPermission() 
    { 
        var request = new PermissionRequest();
        while (request.Status == PermissionStatus.RequestPending) { 
            yield return null;
        }
    }
#endif
}
