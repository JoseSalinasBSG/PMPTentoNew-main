using DataStorage;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AchievementListContainer
{
    public List<AchievementData.Achievement> achievementList = new List<AchievementData.Achievement>();//se crea lista con clase Achievement
}

[CreateAssetMenu(fileName = "AchievementData", menuName = "ScriptableObjects/AchievementData")]
public class AchievementData : ScriptableObject
{
    [Serializable]
    public class Achievement
    {
        public string Name;
        public int ConsecutiveAnswer;
        public int CurrentCounter;
        public int MaxCounter;
        public int CurrentLevel;
        public int MaxLevel;

        //public int MaxCounterDifficulty;
        public int GiftsObtained;
        public List<GiftData.Gift> GiftData;
    }

    public AchievementListContainer achievementListContainer;

    private DataStorageManager _dataStorageManager = new DataStorageManager(new PlayerPrefsStorageAdapter());
    private void OnEnable()
    {
        LoadLocalData();
    }

    public void AddCounter(int index)//contador con parametro index
    {
        var achievement = achievementListContainer.achievementList[index];//accedo al indice index de la lista de clase Achievement osea al SO AchievementData
        achievement.CurrentCounter++;//accedo al contador y lo aumento

        if (achievement.CurrentCounter >= achievement.MaxCounter)//accedo a los campos de la lista y comparo si CurrentCounter es mayor igual que MaxCounter
        {
            achievement.CurrentLevel++;//aumentamos un Nivel
            if (achievement.CurrentLevel>=achievement.MaxLevel)
            {
                achievement.CurrentLevel = 0;

            }
            achievement.CurrentCounter = 0;//reiniciamos
            //AddMaxCounterDifficulty(index);
            AddGiftsObtained(index);
        }
        SaveLocalData();
    }

    /*private void AddMaxCounterDifficulty(int index)
    {
        var achievement = achievementListContainer.achievementList[index];
        achievement.MaxCounter += achievement.MaxCounterDifficulty;
        SaveLocalData();
    }*/

    private void AddGiftsObtained(int index)
    {
        var achievement = achievementListContainer.achievementList[index];// elemento index de la lista
        achievement.GiftsObtained++;
        SaveLocalData();
    }

    public void RemoveGiftsObtained(Achievement achievement)
    {
        achievement.GiftsObtained = 0;
        SaveLocalData();
    }

    private void SaveLocalData()
    {
        // AchievementData data = CreateInstance<AchievementData>();
        // data.achievementList = new List<Achievement>();
        // data.achievementList.AddRange(achievementList);
        //
        // if (FileManager.WriteToFile("SaveData.dat", JsonUtility.ToJson(data)))
        // {
        //     Debug.Log("Save successful");
        // }

        //PlayerPrefs.SetString("AchieveData", JsonUtility.ToJson(achievementListContainer));//guarda la cadena JSON en la clave "Achieve Data"
        //PlayerPrefs.Save();//para que se guarden de inmediato
        _dataStorageManager.Save("AchieveData", achievementListContainer);
    }

    private void LoadLocalData()
    {
        // if (FileManager.LoadFromFile("SaveData.dat", out var json))
        // {
        //     JsonUtility.FromJsonOverwrite(json, this);
        //
        //     Debug.Log("Load complete");
        // }    
        //if (PlayerPrefs.HasKey("AchieveData"))//verifica si existe la clave AchieveData
        if (_dataStorageManager.HasKey("AchieveData"))//verifica si existe la clave AchieveData
        {//si hay datos guardados previamente, se cargaran
            //JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString("AchieveData"), achievementListContainer);//obtiene el valor asociado a la clave como cadena y sobreescribe los datos en
            //                                                                                              //el objeto achievementListContainer con los datos deserializados del JSON obtenido
            JsonUtility.FromJsonOverwrite(_dataStorageManager.Load<string>("AchieveData"), achievementListContainer);
        }
        else
        { 
            SaveLocalData();
        }
    }
    /*
    public void StreakCounter(int verifier,string date, string origin)
    {
        switch (verifier)
        {
            case 4:
                achievementListContainer.achievementList[0].Streak4Questions++;
                achievementListContainer.achievementList[0].Streak4Date = date;
                achievementListContainer.achievementList[0].lastOriginStreak4 = origin;

                Debug.Log("Se a�adio logro racha 4 preguntas");
                break;

            case 6:
                achievementListContainer.achievementList[0].Streak6Questions++;
                achievementListContainer.achievementList[0].Streak6Date = date;
                achievementListContainer.achievementList[0].lastOriginStreak6 = origin;
                Debug.Log("Se a�adio logro racha 6 preguntas");
                break;
            case 8:
                achievementListContainer.achievementList[0].Streak8Questions++;
                achievementListContainer.achievementList[0].Streak8Date = date;
                achievementListContainer.achievementList[0].lastOriginStreak8 = origin;
                Debug.Log("Se a�adio logro racha 8 preguntas");
                break;

            case 10:
                achievementListContainer.achievementList[0].Streak10Questions++;
                achievementListContainer.achievementList[0].Streak10Date = date;
                achievementListContainer.achievementList[0].lastOriginStreak10 = origin;
                Debug.Log("Se a�adio logro racha 10 preguntas");
                break;                
        }
        SaveLocalData();
    }*/
}
