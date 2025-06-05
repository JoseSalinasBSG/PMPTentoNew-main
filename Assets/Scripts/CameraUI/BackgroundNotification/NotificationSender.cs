using System;
using UnityEngine;

public class NotificationSender : MonoBehaviour
{
    [SerializeField] private string idCanal = "canalNotificacion";
    [SerializeField] private string nameCanal = "CanalNotificacion";
    [SerializeField] private string descriptionCanal = "Canal para notificacion";
    [SerializeField] private string titleNotification = "Vuelve a ingresar - test ";
    [SerializeField] private string textNotification = "Notificaci�n para volver a jugar - test ";

    private int counter = 0;

    public void SendNotification()
    {
        DateTime dateTimeActive = DateTime.Now.AddSeconds(5.0f);

        BackgroundNotification backgroundNotification = new BackgroundNotification
        {
            IdCanal = $"{idCanal}{counter}",
            NameCanal = $"{nameCanal}{counter}",
            DescriptionCanal = $"{descriptionCanal}{counter}",
            TitleNotification = $"{titleNotification}{counter}",
            TextNotification = $"{textNotification}{counter}",
            ActivationTime = dateTimeActive
        };

        GameEvents.SendNotification?.Invoke(backgroundNotification);


        BackgroundNotificationIos backgroundNotificationIos = new BackgroundNotificationIos
        {
            Identifier = idCanal,
            Title = titleNotification,
            Body = textNotification,
            Subtitle = nameCanal,
            FireTimeInHours = 0,
            FireTimeInMinutes = 0,
            FireTimeInSeconds = 5
        };
        GameEvents.SendNotificationIos.Invoke(backgroundNotificationIos);

        AddCounter();
    }

    private void AddCounter() 
    {
        counter++;
    }
}
