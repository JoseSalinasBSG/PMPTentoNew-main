using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Notifications : MonoBehaviour
{
    [SerializeField] NotificationData notificationData;
    [SerializeField] private GameObject prefab;//referencia al prefab que tiene contenedores de notificaciones
    [SerializeField] private Transform content;//hace referencia al contenedor de notifiaciones en el scroll view

    public void AddNotificationsToPanel()
    {
        ClearChildrenAchievement();
        var notifications = notificationData.NotificationListContainer.NotificationList;
        var textChild = prefab.GetComponentInChildren<TextMeshProUGUI>();//variable q almacenara el hijo con TextMeshProUGUI
        

        foreach (var item in notifications)
        {
            textChild.SetText(item.Message);
            var message = Instantiate(prefab, content);
            message.GetComponent<NotificationMessage>().SetNotificationPanel(item);
        }
    }

    private void ClearChildrenAchievement()//limpia 
    {
        for (int i = content.childCount - 1; i >= 0; i--)//como un for inverso
        {
            Destroy(content.GetChild(i).gameObject);
        }
    }
}
