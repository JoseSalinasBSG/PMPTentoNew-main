using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class NotificationPanel
{
    public string Title;
    public string Message;
}

[Serializable]
public class NotificationListContainer
{
    public List<NotificationPanel> NotificationList = new List<NotificationPanel>();
}

[CreateAssetMenu(fileName = "NotificationData", menuName = "ScriptableObjects/NotificationData")]
public class NotificationData : ScriptableObject
{
    public NotificationListContainer NotificationListContainer;

    private void OnEnable()
    {
        LoadLocalData();
    }

    public void AddNotification(NotificationPanel notificationPanel)
    {
        NotificationListContainer.NotificationList.Add(notificationPanel);
        SaveLocalData();
    }

    public void RemoveNotification(NotificationPanel notificationPanel)
    { 
        NotificationListContainer.NotificationList.Remove(notificationPanel);
        SaveLocalData();
    }

    private void SaveLocalData()
    {
        // NotificationData data = CreateInstance<NotificationData>();
        // data.NotificationList = new List<NotificationPanel>();
        // data.NotificationList.AddRange(NotificationList);
        //
        // if (FileManager.WriteToFile("SaveData1.dat", JsonUtility.ToJson(data)))
        // {
        //     Debug.Log("Save successful");
        // }
        PlayerPrefs.SetString("NotificationData", JsonUtility.ToJson(NotificationListContainer));
        PlayerPrefs.Save();
    }

    private void LoadLocalData()
    {
        // if (FileManager.LoadFromFile("SaveData1.dat", out var json))
        // {
        //     JsonUtility.FromJsonOverwrite(json, this);
        //
        //     Debug.Log("Load complete");
        // }
        if (PlayerPrefs.HasKey("NotificationData"))
        {
            JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString("NotificationData"), NotificationListContainer);
        }
    }
}
