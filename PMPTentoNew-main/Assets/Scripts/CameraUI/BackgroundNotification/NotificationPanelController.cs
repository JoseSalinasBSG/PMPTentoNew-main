using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NotificationPanelController : MonoBehaviour
{
    [SerializeField] private NotificationData notificationData;

    private void OnEnable()
    {
        GameEvents.AddNotificationPanel += AddNotification;
    }

    private void OnDisable()
    {
        GameEvents.AddNotificationPanel -= AddNotification;
    }

    private void AddNotification(NotificationPanel notificationPanel)
    {
        notificationData.AddNotification(notificationPanel);
    }
}
