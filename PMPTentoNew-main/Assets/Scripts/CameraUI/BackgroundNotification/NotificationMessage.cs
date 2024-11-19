using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationMessage : MonoBehaviour
{
    [SerializeField] private NotificationData notificationData;

    private NotificationPanel _notificationPanel;

    public void SetNotificationPanel(NotificationPanel notificationPanel)
    {
        _notificationPanel = notificationPanel;
    }
    public void RemoveNotification()
    {
        notificationData.RemoveNotification(_notificationPanel);
        Destroy(gameObject);
    }
}
